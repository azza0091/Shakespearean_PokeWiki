using Shakespearean_PokeWiki.Models.Pokemon;
using Shakespearean_PokeWiki.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shakespearean_PokeWiki.Controllers
{
    public class PokemonController : ApiController
    {
        static readonly HttpClient Client = new HttpClient();

        [Route("pokemon/{pokemonName}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPokemonDataAsync(string pokemonName)
        {
            //test api call get pokemon
            var pokeapiEndpoint = ConfigurationManager.AppSettings["PokeApiEndpoint"];
            var shakesperareTranslatorEndpoint = ConfigurationManager.AppSettings["ShakespeareTranslatorEndpoint"];

            //get pokemon data via PokeApi
            var pokemonDescription = "";

            //get pokemon data
            var pokemon = await PokeApiUtils.GetPokemonDataAsync(Client, string.Format("{0}/pokemon/{1}", pokeapiEndpoint, pokemonName));

            if (!string.IsNullOrEmpty(pokemon.species?.Url))
            {
                //get species data
                var pokemonSpecies = await PokeApiUtils.GetPokemonSpeciesDataAsync(Client, pokemon.species.Url);
                
                //filter for english language and group results by version, to take only the most recent values 
                List<string> flavorTexts = pokemonSpecies.flavorTextEntries.Where(t => t.language.Name == "en").GroupBy(g => g.version.Url).LastOrDefault()?.Select(s => s.flavorText).ToList();
                pokemonDescription = string.Join("\n", flavorTexts);
            }

            //get translation via ShakespeareTranslator
            var translatedDescription = "";

            //get translation
            var translation = await ShakespeareTranslatorUtils.GetTranslationAsync(Client, shakesperareTranslatorEndpoint, pokemonDescription);
            translatedDescription = translation.contents.translated;

            var response = new TranslatedPokemonResponseModel(pokemonName, translatedDescription);

            return Ok(response);
        }
    }
}
