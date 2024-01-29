using Microsoft.AspNetCore.Http;

namespace ServiceMiddlewares.Middlewares.ResponseWrapper
{
    public static class ResponseWrapManager
    {
        public static BaseResponse ResponseWrapper(object? result)
        {
            var data = result;

            var response = new BaseResponse
            {
                Data = data,
            };

            return response;
        }
    }
}
