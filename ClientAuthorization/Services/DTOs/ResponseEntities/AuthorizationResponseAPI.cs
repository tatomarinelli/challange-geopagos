using Newtonsoft.Json;

namespace ClientAuthorization.Services.DTOs.ResponseEntities
{
    public class AuthorizationResponseAPI
    {
        [JsonProperty("authorized")]
        public bool Authorized { get; set; }
    }
}
