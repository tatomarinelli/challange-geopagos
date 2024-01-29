using ClientAuthorization.Services.DTOs.ResponseEntities;

namespace ClientAuthorization.DTOs.ResponseEntities
{
    public class AuthorizationResponse
    {
        public AuthorizationResponse(AuthorizationResponseAPI response)
        {
            this.Authorized = response.Authorized;
        }

        public bool Authorized { get; set; }
    }
}
