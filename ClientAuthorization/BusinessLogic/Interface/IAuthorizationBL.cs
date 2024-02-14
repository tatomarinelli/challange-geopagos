using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Modules;

namespace ClientAuthorization.BusinessLogic.Interface
{
    public interface IAuthorizationBL : IModule
    {
        Task<AuthorizationResponse> Payment(AuthorizationRequest request);
        void Reverse(string id);
        void Return();
        ConfirmationResponse Confirm(string id);
        List<PendingOperationResponse> GetPendingOperations();
    }
}
