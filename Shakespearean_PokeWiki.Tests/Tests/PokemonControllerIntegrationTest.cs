using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shakespearean_PokeWiki.Controllers;
using System.Web.Http.Results;
using Shakespearean_PokeWiki.Models.Pokemon;
using System.Threading.Tasks;
using System.Net.Http;

namespace Shakespearean_PokeWiki.Tests.Tests
{
    /// <summary>
    /// Descrizione del riepilogo per PokemonControllerIntegrationTest
    /// </summary>
    [TestClass]
    public class PokemonControllerIntegrationTest
    {
        public PokemonControllerIntegrationTest()
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
            //initialize test
            var pokemonName = "pikachu";
            var controller = new PokemonController();

            //execute
            var result = await controller.GetPokemonDataAsync(pokemonName) as OkNegotiatedContentResult<TranslatedPokemonResponseModel>;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pokemonName, result.Content.name);
        }

        [TestMethod]
        public async Task GetPokemonData_InvalidInput()
        {
            //initialize test
            var pokemonName = "fakepokemon";
            var controller = new PokemonController();

            //execute
            var result = await controller.GetPokemonDataAsync(pokemonName) as NegotiatedContentResult<string>;
            
            //assert
            Assert.IsNotNull(result);
        }
    }
}
