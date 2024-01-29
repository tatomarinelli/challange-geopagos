using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.Modules;
using ClientAuthorization.Services.DTOs.RequestEntities;
using ClientAuthorization.Services.Interface;

namespace ClientAuthorization.BusinessLogic
{
    public class AuthorizationBL : IAuthorizationBL
    {
        private readonly IPaymentProcessorService _PaymentProcessorService;

        public AuthorizationBL() { }
        public AuthorizationBL(IPaymentProcessorService PaymentProcessorService)
        {
            _PaymentProcessorService = PaymentProcessorService;
        }
        public async Task<AuthorizationResponse> Payment(AuthorizationRequest request)
        {
            var requestService = new AuthorizationRequestAPI(request);
            var response = await _PaymentProcessorService.Authorize(requestService);

            return new AuthorizationResponse(response);
        }

        public void Return()
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            throw new NotImplementedException();
        }
    }
}
