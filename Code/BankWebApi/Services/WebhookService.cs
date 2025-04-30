using BankWebApi.Services.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace BankWebApi.Services
{
    public class WebhookService
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly IOptionsSnapshot<WebhookOptions> _options;

        public WebhookService(IOptionsSnapshot<WebhookOptions> options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<HttpResponseMessage> SendHook(string title, string payload)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_options.Value.Uri);

            var uriBuilder = new UriBuilder(httpClient.BaseAddress!);
            var query = QueryString.Create([
                new KeyValuePair<string, string?>(_options.Value.KeyParameter, _options.Value.ApiKey),
            new KeyValuePair<string, string?>(_options.Value.TitleParam, title),
            new KeyValuePair<string, string?>(_options.Value.ContentParam, payload)
            ]);

            uriBuilder.Query = query.ToString();
            var result = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString()));
            result.EnsureSuccessStatusCode();
            return result;
        }
    }
}
