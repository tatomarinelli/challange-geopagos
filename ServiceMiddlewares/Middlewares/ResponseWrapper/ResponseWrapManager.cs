using Microsoft.AspNetCore.Http;

namespace ServiceMiddlewares.Middlewares.ResponseWrapper
{
    public static class ResponseWrapManager
    {
        public static BaseResponse ResponseErrorWrapper(BaseResponse result)
        {
            return result;
        }

        public static BaseResponse ResponseDataWrapper(object? result)
        {
            var data = result;
            var response = new BaseResponse()
            {
                Data = data
            };
            return response;
        }
    }
}
