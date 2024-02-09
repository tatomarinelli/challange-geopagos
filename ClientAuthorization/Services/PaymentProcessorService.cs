using ClientAuthorization.Modules;
using ClientAuthorization.Services.Base;
using ClientAuthorization.Services.DTOs.RequestEntities;
using ClientAuthorization.Services.DTOs.ResponseEntities;
using ClientAuthorization.Services.Interface;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace ClientAuthorization.Services
{
    public class PaymentProcessorService : ApiServicesBase, IPaymentProcessorService 
    {
        private static readonly string serviceName = "PAYMENT_PROCESSOR_URL";
        private readonly string serviceUrl;
        public PaymentProcessorService() 
        {
            // If we locally run tests we have to use the localhost and exposed port.
            serviceUrl = Environment.GetEnvironmentVariable(serviceName) ?? "localhost:1001";   
        }
        public PaymentProcessorService(ILogger logger, IHttpClientFactory clientFactory, IConfiguration config, string serviceConfigName) : base(logger, clientFactory, config, serviceConfigName)
        {
        }

        public async Task<AuthorizationResponseAPI> Authorize(AuthorizationRequestAPI request)
        {
            ApiRequestBase<AuthorizationRequestAPI> requestAPI = new ApiRequestBase<AuthorizationRequestAPI>()
            {
                Body = request,
                Method = HttpMethod.Post,
                Url = $"http://{serviceUrl}/v1/PaymentProcessor/Payment",
            };

            var result = await Execute<ApiResponseBase<AuthorizationResponseAPI>, AuthorizationRequestAPI>(requestAPI);
            return result.Data;
        }
    }
}
