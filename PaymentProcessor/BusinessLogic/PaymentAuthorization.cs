using PaymentProcessor.BusinessLogic.Interface;
using PaymentProcessor.DTOs.RequestEntities;
using PaymentProcessor.DTOs.ResponseEntities;
using PaymentProcessor.Helpers;
using PaymentProcessor.Modules;
using System.Reflection;

namespace PaymentProcessor.BusinessLogic
{
    public class PaymentAuthorizationBL : IPaymentAuthorizationBL
    {
        public PaymentAuthorizationResponse AuthorizePayment(PaymentAuthorizationRequest request)
        {
            if (request == null) { throw new Exception("Request erronea"); }

            return IsValidPayment(request);

        }
        private PaymentAuthorizationResponse IsValidPayment(PaymentAuthorizationRequest request)
        {
            bool isInt = request.TransactionData.Amount.IsInteger();
            return new PaymentAuthorizationResponse(isInt);
        }
    }
}
