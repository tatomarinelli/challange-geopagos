 using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.HostedServices;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Models.Database.Mappers;
using ClientAuthorization.Modules;
using ClientAuthorization.Repository.Interface;
using ClientAuthorization.Services.DTOs.RequestEntities;
using ClientAuthorization.Services.DTOs.ResponseEntities;
using ClientAuthorization.Services.Interface;
using System.Security.Cryptography;
using static ClientAuthorization.Models.Database.DatabaseEnums;
using static ClientAuthorization.Models.Database.OperationStatus;

namespace ClientAuthorization.BusinessLogic
{
    public class AuthorizationBL : IAuthorizationBL
    {
        private readonly IPaymentProcessorService _PaymentProcessorService = null!;
        
        // Background Service
        private readonly OperationService _OperationService = null!;

        public AuthorizationBL () { }
        public AuthorizationBL(IPaymentProcessorService PaymentProcessorService, 
                               OperationService OperationService
                              )
        {
            _PaymentProcessorService = PaymentProcessorService;
            _OperationService = OperationService;
        }
        #region Entrypoints
        public async Task<AuthorizationResponse> Payment(AuthorizationRequest request)
        {
            var requestService = new AuthorizationRequestAPI(request);
            var response = await _PaymentProcessorService.Authorize(requestService);

            if (response.Authorized && ClientNeedsValidation(request.ClientType))
            {
                _ = _OperationService.LogPendingConfirmation(request, response, OperationActionEnum.PAY);
            }
            else
            {
                _ = _OperationService.Log(request, response, OperationActionEnum.PAY);
            }

            return new AuthorizationResponse(response);
        }
        public void Reverse(string id)
        {
            _OperationService.ReverseOperation(id);
        }

        public ConfirmationResponse Confirm(string id)
        {
            var operation = _OperationService.ConfirmOperation(id);
            return new ConfirmationResponse(operation);
        }

        public void Return()
        {
            throw new NotImplementedException();
        }
        public List<PendingOperationResponse> GetPendingOperations()
        {
           return _OperationService.GetPendingOperations().ConvertAll(x => new PendingOperationResponse(x));
        }
#endregion
        private bool ClientNeedsValidation(string? clientType) => clientType == "2";

    }
}
