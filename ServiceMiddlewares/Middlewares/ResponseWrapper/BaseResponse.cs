using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ServiceMiddlewares.Middlewares.ResponseWrapper
{
    public class BaseResponse
    {
        [JsonProperty("data")]
        public object? Data { get; set; }
        [JsonProperty("error")]
        public CustomError? Error { get; set; }

    }
}
