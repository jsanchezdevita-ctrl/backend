using Application.Abstractions.Realtime;
using Web.Api.Infrastructure;
using Web.Api.Realtime;

namespace Web.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddScoped<IDisponibilidadNotifier, DisponibilidadNotifier>();

        return services;
    }
}
