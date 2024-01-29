using ClientAuthorization.Modules;
using ClientAuthorization.Services.DTOs.RequestEntities;
using ClientAuthorization.Services.DTOs.ResponseEntities;

namespace ClientAuthorization.Services.Interface
{
    public interface IPaymentProcessorService : IModule
    {
        Task<AuthorizationResponseAPI> Authorize(AuthorizationRequestAPI request);
    }
}
