using Newtonsoft.Json;
using Shakespearean_PokeWiki.Models.ShakespeareTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Shakespearean_PokeWiki.Shared
{
    public class ShakespeareTranslatorUtils
    {
        /// <summary>
        /// Get Shakespearean translation for a given text
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<ShakespeareTranslator_ResponseModel> GetTranslationAsync(HttpClient client, string endpoint, string text)
        {
            var jsonContent = Helper.CreateRequestContent(new ShakespeareTranslator_InputModel(text));
            var response = await Helper.PostAsync(client, endpoint, jsonContent);

            var translation = JsonConvert.DeserializeObject<ShakespeareTranslator_ResponseModel>(response);

            return translation;
        }
    }
}