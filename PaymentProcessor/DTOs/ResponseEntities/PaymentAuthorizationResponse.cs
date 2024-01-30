using Newtonsoft.Json;

namespace PaymentProcessor.DTOs.ResponseEntities
{
    public class PaymentAuthorizationResponse
    {
        public PaymentAuthorizationResponse(bool isInt)
        {
            this.Authorized = isInt;
        }
        [JsonProperty("authorized")]
        public bool Authorized { get; set; }
    }
}
