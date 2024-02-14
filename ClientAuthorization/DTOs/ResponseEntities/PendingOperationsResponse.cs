using ClientAuthorization.Models.Database;
using ClientAuthorization.Services.DTOs.ResponseEntities;
using Newtonsoft.Json;

namespace ClientAuthorization.DTOs.ResponseEntities
{
    public class PendingOperationResponse
    {
        public PendingOperationResponse(OperationPending op)
        {
            this.OperationId = op.Operation.OperationId.ToString();
            this.ClientId = op.Operation.ClientId;
            this.ExpirationAt = op.ExpiresAt.ToString();
            this.TransactionAmount = op.Operation.TransactionAmount;
        }

        [JsonProperty("OperationID")]
        public string? OperationId { get; set; }
        [JsonProperty("ClientId")]
        public string? ClientId { get; set; }
        [JsonProperty("ExpirationAt")]
        public string? ExpirationAt { get; set; }
        [JsonProperty("TransactionAmount")]
        public decimal? TransactionAmount { get; set; }
    }
}
