using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

//builder.Services.AddHostedService<DbInitializer>();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(3600);
    options.AddHealthCheckEndpoint("Main", "/health");
})
.AddInMemoryStorage();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPoliciesNames.Development, builder =>
    {
        builder
            .WithOrigins("http://localhost:3000",
            "https://login-gamma-mauve.vercel.app",
            "https://control-acceso-admin.vercel.app",
            "https://ucsa-panel-cliente-estacionamiento.vercel.app",
            "http://localhost:3000",
            "https://ucsa-panel-cliente-estacionamiento.vercel.app")
            //.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();

    
}

app.UseCors(CorsPoliciesNames.Development);

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-json";
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

app.MapHub<MonitoreoHub>("/hubs/monitoreo");
app.MapHub<DispositivoEstadoHub>("/hubs/dispositivo-estado");
app.MapHub<DisponibilidadHub>("/hubs/disponibilidad");
app.MapHub<DisponibilidadMobileHub>("/hubs/disponibilidad-mobile");

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}
