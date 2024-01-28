using PaymentProcessor.DTOs.RequestEntities;
using PaymentProcessor.DTOs.ResponseEntities;
using PaymentProcessor.Modules;

namespace PaymentProcessor.BusinessLogic.Interface
{
    public interface IPaymentAuthorizationBL : IModule
    {
        PaymentAuthorizationResponse AuthorizePayment(PaymentAuthorizationRequest request);
    }
}
