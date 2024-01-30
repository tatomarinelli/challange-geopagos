using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.Modules;

namespace ClientAuthorization.HostedServices.Interface
{
    public interface IConfirmAuthorizationService
    {
        public void AddToQueue(AuthorizationRequest request, DateTime expiresAt);
    }
}
