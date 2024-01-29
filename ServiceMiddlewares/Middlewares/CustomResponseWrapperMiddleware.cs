using ServiceMiddlewares.Middlewares.ResponseWrapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceMiddlewares.Middlewares
{
    public class CustomResponseWrapperMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Storing Context Body Response
            var currentBody = context.Response.Body;

            // Using MemoryStream to hold Controller Response
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Passing call to Controller
            await next(context);

            // Resetting Context Body Response
            context.Response.Body = currentBody;

            // Setting Memory Stream Position to Beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Read Memory Stream data to the end
            var readToEnd = new StreamReader(memoryStream).ReadToEnd();

            // Deserializing Controller Response to an object
            var result = JsonConvert.DeserializeObject(readToEnd);
           
            if (context.Response.StatusCode != StatusCodes.Status200OK)
                throw new Exception((result as Newtonsoft.Json.Linq.JObject)?.GetValue("errors")?.ToString());
            

            // Invoking Customizations Method to handle Custom Formatted Response
            var response = ResponseWrapManager.ResponseWrapper(result);

            // return response to caller
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}

