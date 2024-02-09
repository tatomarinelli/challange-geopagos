using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.HostedServices;
using ClientAuthorization.HostedServices.Interface;
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
        private readonly IPaymentProcessorService _PaymentProcessorService;
        private readonly ConfirmAuthorizationService _ConfirmAuthorizationService;

        // DB Repositories
        private readonly IOperationActionRepository _OperationActionRepository;
        private readonly IOperationLogRepository _OperationLogRepository;
        private readonly IOperationPendingRepository _OperationPendingRepository;
        private readonly IOperationStatusRepository _OperationStatusRepository;

        public AuthorizationBL() { }
        public AuthorizationBL(IPaymentProcessorService PaymentProcessorService, 
                               ConfirmAuthorizationService ConfirmAuthorizationService,
                               IOperationActionRepository OperationActionRepository,
                               IOperationLogRepository OperationLogRepository,
                               IOperationPendingRepository OperationPendingRepository,
                               IOperationStatusRepository OperationStatusRepository
                              )
        {
            _PaymentProcessorService = PaymentProcessorService;
            _ConfirmAuthorizationService = ConfirmAuthorizationService;

            _OperationActionRepository = OperationActionRepository;
            _OperationLogRepository = OperationLogRepository;
            _OperationPendingRepository = OperationPendingRepository;
            _OperationStatusRepository = OperationStatusRepository;

        }
        #region Entrypoints
        public async Task<AuthorizationResponse> Payment(AuthorizationRequest request)
        {
            var requestService = new AuthorizationRequestAPI(request);
            var response = await _PaymentProcessorService.Authorize(requestService);

            // OperationLog -> Approved - Denied
            var operation = LogOperation(request, response, OperationActionEnum.PAY);
            if (response.Authorized && ClientNeedsValidation(request.ClientType))
            {
                SetOperationPendingConfirmation(operation);
            }

            return new AuthorizationResponse(response, operation.OperationId.ToString());
        }
        public void Reverse(string id)
        {
            var op = FindPendingOperationDB(id);
            
            // Update log
            op.Operation.OperationStatus = OperationStatusEnum.DEN.ToString();
            op.Operation.OperationAction = OperationActionEnum.REV.ToString();
            _OperationLogRepository.Update(op.Operation);

            // Delete from pending table
            _OperationPendingRepository.Delete(op);

            Console.WriteLine("Operation to reverse: " + op.Operation.OperationId);
        }

        private OperationPending FindPendingOperationDB(string id)
        {
            var op = _OperationPendingRepository.Find(x => x.OperationId.ToString() == id, x => x.Operation).FirstOrDefault();
            return op ?? throw new Exception("Operation not found"); ;
        }

        public ConfirmationResponse Confirm(string id)
        {
            // Remove from cron
            var operation = _ConfirmAuthorizationService.RemoveOperation(id);
            if (operation != null)
            {
                // Write in database log table
                UpdateLogOperationById(id, OperationStatusEnum.AUT);

                // Remove from pending confirmation table
                var op = _OperationPendingRepository.Find(x => x.OperationId.ToString() == id).FirstOrDefault();
                _OperationPendingRepository.Delete(op);

                return new ConfirmationResponse(op.Operation);
            }
            throw new Exception($"Operation not found. Id {id}");
        }

        public void Return()
        {
            throw new NotImplementedException();
        }
#endregion

        private OperationLog LogOperation(AuthorizationRequest request, AuthorizationResponseAPI response, OperationActionEnum action)
        {
            OperationStatusEnum status = response.Authorized ? OperationStatusEnum.AUT : OperationStatusEnum.DEN;
            var operation = OperationLogMapper.Map(request, status, action);

            _OperationLogRepository.Create(operation);
            return operation;
        }
        private void UpdateLogOperationById(string id, OperationStatusEnum status)
        {
            var op = _OperationLogRepository.Find(x => x.OperationId.ToString() == id).FirstOrDefault();
            op.OperationStatus = status.ToString();

            _OperationLogRepository.Update(op);
        }

        private void SetOperationPendingConfirmation(OperationLog operation)
        {
            operation.OperationStatus = OperationStatusEnum.PEN.ToString();
            _OperationLogRepository.Update(operation);

            // add to queue to keep track - Add to pending log
            _ConfirmAuthorizationService.AddOperation(operation.OperationId.ToString(), operation, DateTime.Now.AddSeconds(45));

            var log = OperationPendingMapper.Map(operation.OperationId);
            _OperationPendingRepository.Create(log);
        }

        private bool ClientNeedsValidation(string? clientType) => clientType == "2";

    }
}
