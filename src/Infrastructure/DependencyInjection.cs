using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Notifications;
using Infrastructure.Analytics;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.BackgroundJobs;
using Infrastructure.Database;
using Infrastructure.DomainEvents;
using Infrastructure.Notifications;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using SharedKernel;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase(configuration)
            .AddAnalyticsDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal()
            .AddBackgroundJobs()
            .AddDomainEventHandlers()
            .AddFCM();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddAnalyticsDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("AnalyticsDatabase");

        services.AddDbContext<AnalyticDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IAnalyticsDbContext>(sp => sp.GetRequiredService<AnalyticDbContext>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!, name: "main-db")
            .AddNpgSql(configuration.GetConnectionString("AnalyticsDatabase")!, name: "analytics-db");

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {


        // 1. Configurar opciones de cookies desde appsettings.json
        services.Configure<CookieConfigOptions>(
            configuration.GetSection("CookieConfigOptions"));

        services.AddScoped<ICookieService, CookieService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                // Opcional: Leer token de cookie también
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Intentar obtener token de cookie si no está en header
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.Token = context.Request.Cookies["access_token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UsuarioContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IZonaNotifier, ZonaNotifier>();
        services.AddScoped<IZonaNotifierWeb, ZonaNotifierWeb>();
        services.AddScoped<IMonitoreoNotifier, MonitoreoNotifier>();
        services.AddScoped<IDispositivoEstadoNotifier, DispositivoEstadoNotifier>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var analyticsJobKey = new JobKey("ProcessAnalytics");

            configure
                .AddJob<ProcessAnalyticsJob>(job => job.WithIdentity(analyticsJobKey))
                .AddTrigger(trigger => trigger
                    .ForJob(analyticsJobKey)
                    .WithIdentity("ProcessAnalytics-trigger")
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInMinutes(10)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(DependencyInjection)) // Assembly de Infrastructure
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddFCM(this IServiceCollection services)
    {
        services.AddHttpClient<IFcmNotificationService, FcmNotificationService>(client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        return services;
    }
}
