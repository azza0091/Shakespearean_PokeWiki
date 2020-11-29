using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Shakespearean_PokeWiki.Tests.Model;
using Shakespearean_PokeWiki.Models.ShakespeareTranslator;
using Shakespearean_PokeWiki.Shared;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Shakespearean_PokeWiki.Tests.Tests
{
    /// <summary>
    /// Descrizione del riepilogo per ShakespeareTranslatorUtilsUnitTest
    /// </summary>
    [TestClass]
    public class ShakespeareTranslatorUtilsUnitTest
    {
        private string shakespeareTranslatorEndpoint = ConfigurationManager.AppSettings["ShakespeareTranslatorEndpoint"];

        public ShakespeareTranslatorUtilsUnitTest()
        {
            
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Ottiene o imposta il contesto del test che fornisce
        ///le informazioni e le funzionalità per l'esecuzione del test corrente.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributi di test aggiuntivi
        //
        // È possibile utilizzare i seguenti attributi aggiuntivi per la scrittura dei test:
        //
        // Utilizzare ClassInitialize per eseguire il codice prima di eseguire il primo test della classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilizzare ClassCleanup per eseguire il codice dopo l'esecuzione di tutti i test della classe
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilizzare TestInitialize per eseguire il codice prima di eseguire ciascun test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilizzare TestCleanup per eseguire il codice dopo l'esecuzione di ciascun test
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public async Task GetTranslation_ValidInput()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response.StatusCode = HttpStatusCode.OK;

            var textToTranslate = "sample text to be translated";

            var sampleTranslation = GetSampleTranslation_Success(textToTranslate);
            fakeHandler.Response.Content = new StringContent(JsonConvert.SerializeObject(sampleTranslation), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient(fakeHandler);

            //execute
            var result = await ShakespeareTranslatorUtils.GetTranslationAsync(client, shakespeareTranslatorEndpoint, textToTranslate);

            //assert
            Assert.IsTrue(result.success != null);
            Assert.AreEqual(textToTranslate, result.contents.text);
        }

        [TestMethod]
        public async Task GetTranslation_TooManyRequests()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response = new HttpResponseMessage((HttpStatusCode)429);

            var textToTranslate = "sample text to be translated";

            var sampleTranslation = GetSampleTranslation_Error(textToTranslate);
            fakeHandler.Response.Content = new StringContent(JsonConvert.SerializeObject(sampleTranslation), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient(fakeHandler);

            //execute
            try
            {
                var result = await ShakespeareTranslatorUtils.GetTranslationAsync(client, shakespeareTranslatorEndpoint, textToTranslate);
            }
            catch (HttpException ex)
            {
                Assert.AreEqual((HttpStatusCode)429, (HttpStatusCode)ex.GetHttpCode());
                return;
            }
        }

        public ShakespeareTranslator_ResponseModel GetSampleTranslation_Success(string textToTranslate)
        {
            var translation = new ShakespeareTranslator_ResponseModel
            {
                contents = new ShakespeareTranslator_ResponseModelContent
                {
                    text = textToTranslate,
                    translated = textToTranslate,
                    translation = "shakespeare"
                },
                success = new ShakespeareTranslator_Success
                {
                    total = 1
                }
            };

            return translation;
        }

        public ShakespeareTranslator_ResponseModel GetSampleTranslation_Error(string textToTranslate)
        {
            var translation = new ShakespeareTranslator_ResponseModel
            {
                contents = null,
                success = null,
                error = new ShakespeareTranslator_Error
                {
                    code = 429,
                    message = "Too Many Requests"
                }
            };

            return translation;
        }
    }
}
