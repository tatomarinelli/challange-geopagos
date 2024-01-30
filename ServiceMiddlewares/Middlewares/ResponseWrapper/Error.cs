using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceMiddlewares.Middlewares.ResponseWrapper
{
    public class CustomError
    {
        public CustomError() { }
        public CustomError(Exception ex, int statusCode)
        {
            Title = "Internal error";
            Detail = ex.Message;
            Status = statusCode;
        }
        [JsonProperty("title")]
        public string? Title { get; set; }
        [JsonProperty("detail")]
        public string? Detail { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }
    }
}
