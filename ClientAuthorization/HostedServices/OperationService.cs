using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Models.Database.Mappers;
using ClientAuthorization.Repository.Interface;
using ClientAuthorization.Services.DTOs.ResponseEntities;
using System;
using static ClientAuthorization.Models.Database.DatabaseEnums;

namespace ClientAuthorization.HostedServices
{
    public class OperationService : BackgroundService
    {
        private IServiceScope scope;
        
        private IOperationLogRepository _OperationLogRepository;
        private IOperationPendingRepository _OperationPendingRepository;
        private ConfirmAuthorizationService _ConfirmAuthorizationService;

        private readonly IServiceScopeFactory scopeFactory;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);

        public OperationService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public OperationService()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_period);
            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken)) { }
            return;
        }
        /// <summary>
        /// Will log the operation then validate if client type needs confirmation and if true will set the pending status.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Log(AuthorizationRequest request, AuthorizationResponseAPI response, OperationActionEnum action)
        {
            await Task.Run(() =>
            {
                //await Task.Delay(TimeSpan.FromSeconds(10)); //To test async log add "async" in line 37.. (async () => {...})
                SetDependencies();

                OperationStatusEnum status = response.Authorized ? OperationStatusEnum.AUT : OperationStatusEnum.DEN;
                _Log(request, response, action, status);
            });
            scope.Dispose();
        }
        public async Task LogPendingConfirmation(AuthorizationRequest request, AuthorizationResponseAPI response, OperationActionEnum action)
        {
            await Task.Run(() =>
            {
                SetDependencies();

                // Log to DB - Assign to "operation" to retrieve PK
                var operation = _Log(request, response, action, OperationStatusEnum.PEN);

                // Add to queue to keep track
                AddOperationToPendingQueue(operation);

                // Add to db
                AddOperationToPendingTable(operation);
            });
        }

        public OperationLog ConfirmOperation(string id)
        {
            SetDependencies();
            var operation = RemoveOperationFromPendingQueue(id);
            UpdateOperation(operation, OperationStatusEnum.AUT);
            RemoveOperationFromPendingTable(id);
            return operation;
        }
        public OperationLog ReverseOperation(string id)
        {
            SetDependencies();
            var op = FindOperationInLog(id);
            if (op.OperationStatus == OperationStatusEnum.PEN.ToString())
            {
                RemoveOperationFromPendingQueue(id);
                RemoveOperationFromPendingTable(id);
            }

            // Update log
            UpdateOperation(op, OperationStatusEnum.DEN, OperationActionEnum.REV);
            return op;
        }
        public List<OperationPending> GetPendingOperations()
        {
            SetDependencies();
            return _OperationPendingRepository.GetAllOperations();
        }

        #region Utils
        private OperationLog _Log(AuthorizationRequest request, AuthorizationResponseAPI response, OperationActionEnum action, OperationStatusEnum status)
        {
            var operation = OperationLogMapper.Map(request, status, action); // DB Object
            _OperationLogRepository.Create(operation);

            return operation;
        }
        private void AddOperationToPendingTable(OperationLog operation)
        {
            var log = OperationPendingMapper.Map(operation.OperationId);
            _OperationPendingRepository.Create(log);
        }

        private void AddOperationToPendingQueue(OperationLog operation)
        {
            _ConfirmAuthorizationService.AddOperation(operation.OperationId.ToString(), operation, DateTime.Now.AddSeconds(45));
        }

        private void RemoveOperationFromPendingTable(string id)
        {
            // Remove from pending confirmation table
            var op = _OperationPendingRepository.Find(x => x.OperationId.ToString() == id).FirstOrDefault() 
                     ?? throw new Exception($"Operation {id} not found");
            _OperationPendingRepository.Delete(op);
        }
        private OperationLog RemoveOperationFromPendingQueue(string id)
        {
            return _ConfirmAuthorizationService.RemoveOperation(id);
        }

        private void UpdateOperation(OperationLog operation, OperationStatusEnum? newStatus = null, OperationActionEnum? newAction = null)
        {
            operation.OperationAction = newAction != null ? newAction.ToString() : operation.OperationAction;
            operation.OperationStatus = newStatus != null ? newStatus.ToString() : operation.OperationStatus;
            _OperationLogRepository.Update(operation);
        }

        private OperationLog FindOperationInLog(string id)
        {
            return _OperationLogRepository.Find(x => x.OperationId.ToString() == id).FirstOrDefault()
                   ?? throw new Exception($"Operation {id} not found");
        }
        #endregion
        private void SetDependencies()
        {
            scope = scopeFactory.CreateScope();
            _OperationLogRepository = scope.ServiceProvider.GetRequiredService<IOperationLogRepository>();
            _OperationPendingRepository = scope.ServiceProvider.GetRequiredService<IOperationPendingRepository>();

            _ConfirmAuthorizationService = scope.ServiceProvider.GetRequiredService<ConfirmAuthorizationService>();
        }
    }
}
