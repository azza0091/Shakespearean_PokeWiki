using Newtonsoft.Json;
using Shakespearean_PokeWiki.Models.PokeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Shakespearean_PokeWiki.Shared
{
    public static class PokeApiUtils
    {
        /// <summary>
        /// Get Pokemon data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<PokemonResponseModel> GetPokemonDataAsync(HttpClient client, string endpoint)
        {
            var response = await Helper.GetAsync(client, endpoint);
            var pokemon = JsonConvert.DeserializeObject<PokemonResponseModel>(response);

            return pokemon;
        }

        /// <summary>
        /// Get Pokemon species data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<SpeciesReponseModel> GetPokemonSpeciesDataAsync(HttpClient client, string endpoint)
        {
            var response = await Helper.GetAsync(client, endpoint);
            var pokemonSpecies = JsonConvert.DeserializeObject<SpeciesReponseModel>(response);

            return pokemonSpecies;
        }
    }
}