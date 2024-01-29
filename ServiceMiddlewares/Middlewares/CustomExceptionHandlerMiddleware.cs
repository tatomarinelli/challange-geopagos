using ServiceMiddlewares.Middlewares.ResponseWrapper;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ServiceMiddlewares.Middlewares
{
    public class CustomExceptionHandlerMiddleware : IMiddleware
    {
        /*
        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(ILogger logger)
        {
            _logger = logger;
        }*/

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context); 
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                CustomError error = new CustomError(ex, context.Response.StatusCode);

                string json = JsonConvert.SerializeObject(error);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);

            }
        }
    }
}
