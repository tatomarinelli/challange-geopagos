using ClientAuthorization.HostedServices;
using ClientAuthorization.Models.Database;
using ClientAuthorization.Modules;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using ServiceMiddlewares.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<CustomResponseWrapperMiddleware>();
builder.Services.AddTransient<CustomExceptionHandlerMiddleware>();


builder.Services.AddHostedService<ConfirmAuthorizationService>();
builder.Services.AddSingleton<ConfirmAuthorizationService>();
builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<ConfirmAuthorizationService>());

builder.Services.AddHostedService<OperationService>();
builder.Services.AddSingleton<OperationService>();
builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<OperationService>());

builder.Services.RegisterModules();


builder.Services.AddProblemDetails();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region connectionString

string source = "database";
string sourcePort = "5432";
string database = "geopagos_db";
string usuarioDB = "postgres";
string passwordDB = "postgres";

//if (!(usuarioDB.HasValue() && passwordDB.HasValue() && source.HasValue() && sourcePort.HasValue()))
//    throw new Exception("Faltan configurar elementos del ConnectionString.");

string connectionString = string.Format("Host={0}:{1};Database={2};Username={3};Password={4}", source, sourcePort, database, usuarioDB, passwordDB);
#endregion connectionString

builder.Services.AddDbContext<geopagos_dbContext>(options => options.UseNpgsql(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseProblemDetails();
app.UseMiddleware<CustomResponseWrapperMiddleware>();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
