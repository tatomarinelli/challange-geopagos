using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessor.BusinessLogic.Interface;
using PaymentProcessor.DTOs.RequestEntities;
using PaymentProcessor.DTOs.ResponseEntities;
using PaymentProcessor.Helpers;

namespace PaymentProcessor.Controllers
{
    [Route("v1/PaymentProcessor/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentAuthorizationBL _PaymentAuthorizationBL;

        public PaymentController(IPaymentAuthorizationBL PaymentAuthorizationBL)
        {
            _PaymentAuthorizationBL = PaymentAuthorizationBL;
        }

        [HttpPost]
        public PaymentAuthorizationResponse Authorize(PaymentAuthorizationRequest request)
        {
            return _PaymentAuthorizationBL.AuthorizePayment(request);
        }
    }
}
