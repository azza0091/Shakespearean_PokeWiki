using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using Shakespearean_PokeWiki.Shared;
using Newtonsoft.Json;
using Shakespearean_PokeWiki.Models.ShakespeareTranslator;

namespace Shakespearean_PokeWiki.Tests.Tests
{
    /// <summary>
    /// Descrizione del riepilogo per ShakespeareTranslatorIntegrationTest
    /// </summary>
    [TestClass]
    public class ShakespeareTranslatorIntegrationTest
    {
        private static HttpClient Client;

        public ShakespeareTranslatorIntegrationTest()
        {

        }

        [ClassInitialize]
        public static void ClassInit(TestContext Context)
        {
            var endpoint = ConfigurationManager.AppSettings["ShakespeareTranslatorEndpoint"];

            Client = new HttpClient();
            Client.BaseAddress = new Uri(endpoint);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Client.Dispose();
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
        public async Task ShakespeareTranslator_ValidInput()
        {
            //initialize test
            var textToTranslate = "sample text to be translated";
            //var jsonContent = Helper.CreateRequestContent(new ShakespeareTranslator_InputModel(textToTranslate));

            //execute
            var translation = await ShakespeareTranslatorUtils.GetTranslationAsync(Client, Client.BaseAddress.ToString(), textToTranslate);
            //var response = await Client.PostAsync("", jsonContent);
            //var responseContent = await response.Content.ReadAsStringAsync();
            //var translation = JsonConvert.DeserializeObject<ShakespeareTranslator_ResponseModel>(responseContent);

            //assert
            //response.EnsureSuccessStatusCode();
            Assert.IsFalse(string.IsNullOrEmpty(translation.contents.translated));
        }
    }
}
