using Newtonsoft.Json;
using ServiceMiddlewares.Middlewares.ResponseWrapper;

namespace ClientAuthorization.Services.Base
{
    public class ApiResponseBase<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("error")]
        public CustomError Error { get; set; }
    }
}
