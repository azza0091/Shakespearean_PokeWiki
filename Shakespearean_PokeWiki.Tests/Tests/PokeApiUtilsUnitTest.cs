using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Shakespearean_PokeWiki.Tests.Model;
using Shakespearean_PokeWiki.Models.PokeApi;
using Newtonsoft.Json;
using Shakespearean_PokeWiki.Shared;
using System.Configuration;
using System.IO;
using System.Web;
using System.Linq;

namespace Shakespearean_PokeWiki.Tests.Tests
{
    /// <summary>
    /// Descrizione del riepilogo per PokeApiUtilsUnitTest
    /// </summary>
    [TestClass]
    public class PokeApiUtilsUnitTest
    {
        private string pokeApiBaseEndpoint = ConfigurationManager.AppSettings["PokeApiEndpoint"];
        private const string pokemonEndpoint = "pokemon";
        private const string pokemonSpeciesEndpoint = "pokemon-species";

        public PokeApiUtilsUnitTest()
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
        public async Task GetPokemonData_ValidInput()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response.StatusCode = HttpStatusCode.OK;

            var pokemonName = "pikachu";
            var endpoint = string.Format("{0}/{1}/{2}", pokeApiBaseEndpoint, pokemonEndpoint, pokemonName);

            var samplePokemon = GetSamplePokemon(pokemonName);
            fakeHandler.Response.Content = new StringContent(JsonConvert.SerializeObject(samplePokemon), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient(fakeHandler);

            //execute
            var result = await PokeApiUtils.GetPokemonDataAsync(client, endpoint);

            //assert
            Assert.AreEqual(pokemonName, result.name);
        }

        [TestMethod]
        public async Task GetPokemonData_InvalidInput()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            //fakeHandler.Response.StatusCode = HttpStatusCode.NotFound;
            fakeHandler.Response.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes("Not Found")));

            var pokemonName = "fakePokemon";
            var endpoint = string.Format("{0}/{1}/{2}", pokeApiBaseEndpoint, pokemonEndpoint, pokemonName);

            HttpClient client = new HttpClient(fakeHandler);

            //execute - assert
            try
            {
                var result = await PokeApiUtils.GetPokemonDataAsync(client, endpoint);
            }
            catch (HttpException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)ex.GetHttpCode());
                return;
            }
        }

        [TestMethod]
        public async Task GetPokemonSpeciesData_ValidInput()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response.StatusCode = HttpStatusCode.OK;

            var sampleSpecies = GetSamplePokemonSpecies();
            fakeHandler.Response.Content = new StringContent(JsonConvert.SerializeObject(sampleSpecies), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient(fakeHandler);

            var endpoint = string.Format("{0}/{1}/1", pokeApiBaseEndpoint, pokemonSpeciesEndpoint);

            //execute
            var result = await PokeApiUtils.GetPokemonSpeciesDataAsync(client, endpoint);

            //assert
            Assert.IsTrue(result.flavorTextEntries.Any());
        }

        [TestMethod]
        public async Task GetPokemonSpeciesData_InvalidInput()
        {
            //mock fake response and initialize test
            var fakeHandler = new FakeHttpMessageHandler();
            fakeHandler.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            //fakeHandler.Response.StatusCode = HttpStatusCode.NotFound;
            fakeHandler.Response.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes("Not Found")));

            HttpClient client = new HttpClient(fakeHandler);

            var endpoint = string.Format("{0}/{1}/0", pokeApiBaseEndpoint, pokemonEndpoint);

            //execute - assert
            try
            {
                var result = await PokeApiUtils.GetPokemonSpeciesDataAsync(client, endpoint);
            }
            catch (HttpException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)ex.GetHttpCode());
                return;
            }
        }

        private PokemonResponseModel GetSamplePokemon(string pokemonName)
        {
            var pokemonId = 25;

            var pokemon = new PokemonResponseModel
            {
                id = pokemonId,
                name = pokemonName,
                species = new NamedAPIResource(pokemonName, string.Format("https://pokeapi.co/api/v2/pokemon-species/{0}/", pokemonId))
            };

            return pokemon;
        }

        private SpeciesReponseModel GetSamplePokemonSpecies()
        {
            var flavorTextEntries = new List<FlavorTextEntryDetail>();
            //add a bunch of sample values
            flavorTextEntries.Add(new FlavorTextEntryDetail
            {
                flavorText = "sample en description 1",
                language = new NamedAPIResource("en", "https://pokeapi.co/api/v2/language/9/"),
                version = new NamedAPIResource("red", "https://pokeapi.co/api/v2/version/1/")
            });
            flavorTextEntries.Add(new FlavorTextEntryDetail
            {
                flavorText = "sample it description 1",
                language = new NamedAPIResource("it", "https://pokeapi.co/api/v2/language/8/"),
                version = new NamedAPIResource("red", "https://pokeapi.co/api/v2/version/1/")
            });
            flavorTextEntries.Add(new FlavorTextEntryDetail
            {
                flavorText = "sample en description 2",
                language = new NamedAPIResource("en", "https://pokeapi.co/api/v2/language/9/"),
                version = new NamedAPIResource("red", "https://pokeapi.co/api/v2/version/1/")
            });

            var pokemonSpecies = new SpeciesReponseModel
            {
                flavorTextEntries = flavorTextEntries
            };

            return pokemonSpecies;
        }
    }
}
