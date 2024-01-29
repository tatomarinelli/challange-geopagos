using ClientAuthorization.Helpers;
using ClientAuthorization.Services.Base;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace ClientAuthorization.Services.Base
{
    public class ApiServicesBase
    {
        protected readonly ILogger _logger;

        public ApiServicesBase() { }

        public ApiServicesBase(ILogger logger, IHttpClientFactory clientFactory, IConfiguration config, String serviceConfigName)
        {
            _logger = logger;
        }

        public async Task<APIResponse> Execute<APIResponse, T>(ApiRequestBase<T> apiRequest) where APIResponse : class where T : class
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(StringHelper.ValidateURL(apiRequest.Url));

            if (apiRequest.Headers is not null)
            {
                foreach (var header in apiRequest.Headers)
                {
                    if (client.DefaultRequestHeaders.Contains(header.Key))
                    {
                        client.DefaultRequestHeaders.Remove(header.Key);
                    }
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            HttpResponseMessage response = new();
            switch (apiRequest.Method)
            {
                case HttpMethod.Get:
                    response = await client.GetAsync(string.Empty);
                    break;
                case HttpMethod.Delete:
                    response = await client.DeleteAsync(string.Empty);
                    break;
                case HttpMethod.Post:
                    response = await client.PostAsync(string.Empty,
                        new StringContent(apiRequest.Body.GetJSON(), System.Text.Encoding.UTF8, "application/json"));
                    break;
                case HttpMethod.Put:
                    response = await client.PutAsync(string.Empty,
                        new StringContent(apiRequest.Body.GetJSON(), System.Text.Encoding.UTF8, "application/json"));
                    break;

            }

            // TODO: Handle error middleware
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());
            else 
                throw new Exception(response.StatusCode.ToString());
        }
    }
}