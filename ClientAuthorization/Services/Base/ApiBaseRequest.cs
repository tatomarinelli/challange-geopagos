
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace ClientAuthorization.Services.Base
{
    public class ApiRequestBase
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }

    public class ApiRequestBase<T> : ApiRequestBase
    {
        public ApiRequestBase()
        {

        }
        public ApiRequestBase(ApiRequestBase request, HttpMethod method, string url, T body)
        {
            Headers = request.Headers;
            Method = method;
            Url = url;
            Body = body;
        }
        public HttpMethod Method { get; set; }
        public string Url { get; set; }
        public T Body { get; set; }
    }
}