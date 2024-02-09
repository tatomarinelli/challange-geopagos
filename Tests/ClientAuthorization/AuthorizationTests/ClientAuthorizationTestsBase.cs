
using ClientAuthorization.BusinessLogic;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.Controllers;
using ClientAuthorization.HostedServices;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Repository.Interface;
using ClientAuthorization.Services;
using ClientAuthorization.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class ClientAuthorizationTestsBase
    {
        protected readonly AuthorizationController _authorizationController;
        protected readonly IAuthorizationBL _authorizationBL;
        protected readonly IPaymentProcessorService _paymentProcessorService;
        private readonly ConfirmAuthorizationService _ConfirmAuthorizationService;

        // DB Repositories
        private readonly IOperationActionRepository _OperationActionRepository;
        private readonly IOperationLogRepository _OperationLogRepository;
        private readonly IOperationPendingRepository _OperationPendingRepository;
        private readonly IOperationStatusRepository _OperationStatusRepository;

        private readonly ILogger logger;
        private readonly DbContext dbContext;

        public ClientAuthorizationTestsBase()
        {
            _paymentProcessorService = new PaymentProcessorService();
            _ConfirmAuthorizationService = new ConfirmAuthorizationService();

            var dbContextOptions = new DbContextOptionsBuilder<geopagos_dbContext>()
                            .UseNpgsql("Host=localhost:5432;Database=geopagos_db;Username=postgres;Password=postgres")
                            .Options;
            var db = new geopagos_dbContext(dbContextOptions);

            _OperationActionRepository = new OperationActionRepository(db, null);
            _OperationLogRepository = new OperationLogRepository(db, null);
            _OperationPendingRepository = new OperationPendingRepository(db, null);
            _OperationStatusRepository = new OperationStatusRepository(db, null);

            _authorizationBL = new AuthorizationBL(_paymentProcessorService,
                                                   _ConfirmAuthorizationService,
                                                   _OperationActionRepository,
                                                   _OperationLogRepository,
                                                   _OperationPendingRepository,
                                                   _OperationStatusRepository);

            _authorizationController = new AuthorizationController(_authorizationBL);
        }
    }
}
