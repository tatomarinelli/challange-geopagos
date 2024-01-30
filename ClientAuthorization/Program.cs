using ClientAuthorization.HostedServices;
using ClientAuthorization.Modules;
using Hellang.Middleware.ProblemDetails;
using ServiceMiddlewares.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<CustomResponseWrapperMiddleware>();
builder.Services.AddTransient<CustomExceptionHandlerMiddleware>();

builder.Services.AddHostedService<ConfirmAuthorizationService>();
builder.Services.AddSingleton<ConfirmAuthorizationService>();
builder.Services.AddSingleton<IHostedService>(p => p.GetRequiredService<ConfirmAuthorizationService>());


builder.Services.RegisterModules();
builder.Services.AddProblemDetails();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
