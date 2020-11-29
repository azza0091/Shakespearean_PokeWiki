using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shakespearean_PokeWiki.Models.PokeApi;
using System.Linq;

namespace Shakespearean_PokeWiki.Tests.Tests
{
    /// <summary>
    /// Descrizione del riepilogo per PokeApiIntegrationTest
    /// </summary>
    [TestClass]
    public class PokeApiIntegrationTest
    {
        private static HttpClient Client;
        private const string pokemonEndpoint = "pokemon";
        private const string pokemonSpeciesEndpoint = "pokemon-species";

        public PokeApiIntegrationTest()
        {
            
        }

        [ClassInitialize]
        public static void ClassInit(TestContext Context)
        {
            var endpoint = ConfigurationManager.AppSettings["PokeApiEndpoint"];

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
        public async Task PokeApi_Pokemon_ValidInput()
        {
            //initialize test
            var pokemonName = "pikachu";
            var request = string.Format("{0}/{1}", pokemonEndpoint, pokemonName);

            //execute
            var response = await Client.GetAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var pokemon = JsonConvert.DeserializeObject<PokemonResponseModel>(responseContent);

            //assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(pokemonName, pokemon.name);
            Assert.IsFalse(string.IsNullOrEmpty(pokemon.species.Url));
        }

        [TestMethod]
        public async Task PokeApi_Pokemon_InvalidInput()
        {
            //initialize test
            var request = string.Format("{0}/fakepokemon", pokemonEndpoint);

            //execute
            var response = await Client.GetAsync(request);

            //assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task PokeApi_Species_ValidInput()
        {
            //initialize test
            var request = string.Format("{0}/1", pokemonSpeciesEndpoint);

            //execute
            var response = await Client.GetAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var species = JsonConvert.DeserializeObject<SpeciesReponseModel>(responseContent);

            //assert
            response.EnsureSuccessStatusCode();
            Assert.IsTrue(species.flavorTextEntries.Any());
        }

        [TestMethod]
        public async Task PokeApi_Species_InvalidInput()
        {
            //initialize test
            var request = string.Format("{0}/0", pokemonSpeciesEndpoint);

            //execute
            var response = await Client.GetAsync(request);

            //assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
    }
}
