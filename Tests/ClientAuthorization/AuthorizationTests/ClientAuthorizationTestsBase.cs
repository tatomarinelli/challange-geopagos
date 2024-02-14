
using ClientAuthorization.BusinessLogic;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.Controllers;
using ClientAuthorization.HostedServices;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Modules;
using ClientAuthorization.Repository.Interface;
using ClientAuthorization.Services;
using ClientAuthorization.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class ClientAuthorizationTestsBase
    {
        protected readonly AuthorizationController _authorizationController;

        protected readonly IAuthorizationBL _authorizationBL;
        protected readonly IPaymentProcessorService _paymentProcessorService;

        private readonly IServiceScopeFactory _ServiceScopeFactory;
        private readonly OperationService _OperationService;


        public ClientAuthorizationTestsBase()
        {
            
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddHostedService<ConfirmAuthorizationService>();
            builder.Services.AddSingleton<ConfirmAuthorizationService>();
            builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<ConfirmAuthorizationService>());
            builder.Services.AddHostedService<OperationService>();
            builder.Services.AddSingleton<OperationService>();
            builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<OperationService>());

            builder.Services.RegisterModules();
            #region connectionString

            string source = "localhost";
            string sourcePort = "5432";
            string database = "geopagos_db";
            string usuarioDB = "postgres";
            string passwordDB = "postgres";

            //if (!(usuarioDB.HasValue() && passwordDB.HasValue() && source.HasValue() && sourcePort.HasValue()))
            //    throw new Exception("Faltan configurar elementos del ConnectionString.");

            string connectionString = string.Format("Host={0}:{1};Database={2};Username={3};Password={4}", source, sourcePort, database, usuarioDB, passwordDB);
            #endregion connectionString

            builder.Services.AddDbContext<geopagos_dbContext>(options => options.UseNpgsql(connectionString));

            ServiceProvider = builder.Services.BuildServiceProvider();

            _paymentProcessorService = ServiceProvider.GetService<IPaymentProcessorService>();
            _OperationService = ServiceProvider.GetService<OperationService>();

            _authorizationBL = new AuthorizationBL(_paymentProcessorService,
                                                   _OperationService);

            _authorizationController = new AuthorizationController(_authorizationBL);
        }
        public ServiceProvider ServiceProvider { get; private set; }
    }
}
