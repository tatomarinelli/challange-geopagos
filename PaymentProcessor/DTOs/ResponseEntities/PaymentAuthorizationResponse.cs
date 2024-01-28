namespace PaymentProcessor.DTOs.ResponseEntities
{
    public class PaymentAuthorizationResponse
    {
        public PaymentAuthorizationResponse(bool isInt)
        {
            this.Authorized = isInt;
        }

        public bool Authorized { get; set; }
    }
}
