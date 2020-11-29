using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shakespearean_PokeWiki.Shared
{
    public static class Helper
    {
        /// <summary>
        /// Construct request body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static HttpContent CreateRequestContent<T>(T serializableObject, Encoding encoding = null, string mediaType = null)
        {
            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(serializableObject, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }), encoding ?? Encoding.UTF8, mediaType ?? "application/json");

            return requestContent;
        }

        /// <summary>
        /// Perform get request and return content
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<ServerResponse> GetAsync(HttpClient client, string endpoint)
        {
            var response = await client.GetAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            return new ServerResponse(response, responseContent);
        }

        /// <summary>
        /// Perform post request and return content
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<ServerResponse> PostAsync(HttpClient client, string endpoint, HttpContent content)
        {
            var response = await client.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            return new ServerResponse(response, responseContent);
        }
    }

    public class ServerResponse
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseContent { get; set; }

        public ServerResponse() { }
        public ServerResponse(HttpResponseMessage response, string responseContent)
        {
            ResponseCode = response.StatusCode;
            ResponseMessage = response.ReasonPhrase;
            ResponseContent = responseContent;
        }
    }
}