using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shakespearean_PokeWiki.Tests.Model
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public FakeHttpMessageHandler()
        {
            this.Response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public HttpResponseMessage Response { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return this.Response;
            });
        }
    }
}
