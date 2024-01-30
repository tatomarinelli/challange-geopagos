
using ClientAuthorization.BusinessLogic;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.Controllers;
using ClientAuthorization.HostedServices;
using ClientAuthorization.Services;
using ClientAuthorization.Services.Interface;

namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class ClientAuthorizationTestsBase
    {
        protected readonly AuthorizationController _authorizationController;
        protected readonly IAuthorizationBL _authorizationBL;
        protected readonly IPaymentProcessorService _paymentProcessorService;
        private readonly ConfirmAuthorizationService _ConfirmAuthorizationService;
        public ClientAuthorizationTestsBase()
        {
            _paymentProcessorService = new PaymentProcessorService();
            _ConfirmAuthorizationService = new ConfirmAuthorizationService();  

            _authorizationBL = new AuthorizationBL(_paymentProcessorService, _ConfirmAuthorizationService);
            _authorizationController = new AuthorizationController(_authorizationBL);
        }
    }
}
