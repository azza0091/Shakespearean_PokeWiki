using Shakespearean_PokeWiki.Models.Pokemon;
using Shakespearean_PokeWiki.Shared;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Shakespearean_PokeWiki.Controllers
{
    public class PokemonController : ApiController
    {
        static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// Get Pokemon description in Shakespearean style
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <response code="200">Operation successful</response>
        /// <returns></returns>
        [Route("pokemon/{pokemonName}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Operation successul")]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Element not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Generic error. An error description is returned")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPokemonDataAsync(string pokemonName)
        {
            //test api call get pokemon
            var pokeapiEndpoint = ConfigurationManager.AppSettings["PokeApiEndpoint"];
            var shakesperareTranslatorEndpoint = ConfigurationManager.AppSettings["ShakespeareTranslatorEndpoint"];

            //get pokemon data via PokeApi
            var pokemonDescription = "";

            //get pokemon data
            try
            {
                var pokemon = await PokeApiUtils.GetPokemonDataAsync(Client, string.Format("{0}/pokemon/{1}", pokeapiEndpoint, pokemonName));

                if (!string.IsNullOrEmpty(pokemon.species?.Url))
                {
                    //get species data
                    var pokemonSpecies = await PokeApiUtils.GetPokemonSpeciesDataAsync(Client, pokemon.species.Url);
                
                    //filter for english language and group results by version, to take only the most recent values 
                    List<string> flavorTexts = pokemonSpecies.flavorTextEntries.Where(t => t.language.Name == "en").GroupBy(g => g.version.Url).LastOrDefault()?.Select(s => s.flavorText).ToList();
                    pokemonDescription = string.Join("\n", flavorTexts);
                }
            }
            catch (HttpException ex)
            {
                return Content((HttpStatusCode)ex.GetHttpCode(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }

            //get translation via ShakespeareTranslator
            var translatedDescription = "";

            //get translation
            try
            {
                var translation = await ShakespeareTranslatorUtils.GetTranslationAsync(Client, shakesperareTranslatorEndpoint, pokemonDescription);
                translatedDescription = translation.contents.translated;
            }
            catch (HttpException ex)
            {
                return Content((HttpStatusCode)ex.GetHttpCode(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }

            var response = new TranslatedPokemonResponseModel(pokemonName, translatedDescription);

            return Ok(response);
        }
    }
}
