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
            
            object? unhandledResult = JsonConvert.DeserializeObject(readToEnd);
            BaseResponse result = null;
            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                result = ResponseWrapManager.ResponseDataWrapper(new object[] { unhandledResult });
            }
            else
            {
                result = JsonConvert.DeserializeObject<BaseResponse?>(readToEnd);
                // Unhandled error -> map to a new CustomError using an exception and reading the errors object from net response.
                // Known response, we map the errors from the service to our wrapper.
                if (result?.Error == null)
                {
                    var ex = new Exception((unhandledResult as Newtonsoft.Json.Linq.JObject)?.GetValue("errors")?.ToString());
                    result.Error = new CustomError(ex, 500);
                }
                result = ResponseWrapManager.ResponseErrorWrapper(result);
            }

            // return response to caller
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}

