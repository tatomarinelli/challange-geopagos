using ClientAuthorization.Services.DTOs.ResponseEntities;

namespace ClientAuthorization.DTOs.ResponseEntities
{
    public class AuthorizationResponse
    {
        public AuthorizationResponse(AuthorizationResponseAPI response, string operationId)
        {
            this.Authorized = response.Authorized;
            this.OperationId = operationId;
        }

        public bool Authorized { get; set; }
        public string OperationId { get; set; }
    }
}
