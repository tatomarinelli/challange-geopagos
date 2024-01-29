using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceMiddlewares.Middlewares.ResponseWrapper
{
    public class CustomError
    {
        public CustomError(Exception ex, int statusCode)
        {
            Error.Title = "Internal error";
            Error.Detail = ex.Message;
            Error.Status = statusCode;
        }
        [JsonProperty("error")]
        public ProblemDetails Error { get; set; } = new ProblemDetails();
    }
}
