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
    public class ConfirmAuthorizationService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;

        private readonly object semaphore = new object();

        public ConfirmAuthorizationService() { }
        public ConfirmAuthorizationService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        private int executionCount = 0;
        private Timer? _timer = null;

        private Dictionary<string, (OperationLog, DateTime)> operationDictionary = new Dictionary<string, (OperationLog, DateTime)>();

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service running.");
            
            _timer = new Timer(CheckExpirationTime, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public void AddOperation(string id, OperationLog operation, DateTime expiresAt)
        {
            operationDictionary.Add(id, new(operation, expiresAt));
        }
        public OperationLog RemoveOperation(string id)
        {
            // TODO: Test race condition?
            lock (semaphore)
            {
                if (operationDictionary.ContainsKey(id))
                { 
                    var operation = operationDictionary[id];
                    operationDictionary.Remove(id);
                    return operation.Item1;
                }
                // Log warning
                throw new Exception($"Operation not found {id}");
            }

        }
        private void CheckExpirationTime(object? state)
        {
            lock (semaphore)
            {
                if (operationDictionary.Count <= 0) return;
                var operation = operationDictionary.First();
                var expirationTime = operation.Value.Item2;
                if (expirationTime <= DateTime.Now)
                {
                    var operationToReverse = operation.Value.Item1;
                    using (var scope = scopeFactory.CreateScope())
                    {
                        //var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                        var AuthorizationBL = scope.ServiceProvider.GetRequiredService<IAuthorizationBL>();
                        AuthorizationBL.Reverse(operation.Key);
                        operationDictionary.Remove(operation.Key);
                    }
                }
            }
        }
    }
}
