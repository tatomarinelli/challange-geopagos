using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.HostedServices;
using ClientAuthorization.HostedServices.Interface;
using ClientAuthorization.Modules;
using ClientAuthorization.Services.DTOs.RequestEntities;
using ClientAuthorization.Services.Interface;
using System.Security.Cryptography;

namespace ClientAuthorization.BusinessLogic
{
    public class AuthorizationBL : IAuthorizationBL
    {
        private readonly IPaymentProcessorService _PaymentProcessorService;
        private readonly ConfirmAuthorizationService _ConfirmAuthorizationService;

        public AuthorizationBL() { }
        public AuthorizationBL(IPaymentProcessorService PaymentProcessorService, ConfirmAuthorizationService ConfirmAuthorizationService)
        {
            _PaymentProcessorService = PaymentProcessorService;
            _ConfirmAuthorizationService = ConfirmAuthorizationService;
        }
        public async Task<AuthorizationResponse> Payment(AuthorizationRequest request)
        {
            var requestService = new AuthorizationRequestAPI(request);
            var response = await _PaymentProcessorService.Authorize(requestService);

            // LOG TO HISTORY -> Approved - Denied
            if (response.Authorized && ClientNeedsValidation(request.ClientType))
            {
                SetOperationPendingConfirmation(request);
            }

            return new AuthorizationResponse(response);
        }

        private void SetOperationPendingConfirmation(AuthorizationRequest request)
        {
            // Add to pending log - retrieve pk - add to queue to keep track
            _ConfirmAuthorizationService.AddOperation(RandomNumberGenerator.GetInt32(10000000).ToString(), request, DateTime.Now.AddSeconds(45));
        }

        private bool ClientNeedsValidation(string? clientType) => clientType == "2";

        public void Return()
        {
            throw new NotImplementedException();
        }
        public void Reverse(AuthorizationRequest operationToReverse)
        {
            // Write in database
            Console.WriteLine("Operation to reverse: " + operationToReverse.TransactionData.Amount);
        }

        public Task<string> Confirm(string id)
        {
            var operation = _ConfirmAuthorizationService.RemoveOperation(id);
            if (operation != null)
            {
                // Write in database history table
                // Remove from pending confirmation table
                return Task.FromResult(id);
            }
            throw new Exception($"Operation not found. Id {id}");
        }
    }
}
