namespace Application.Abstractions.Notifications;

public interface IMonitoreoNotifier
{
    Task NotificarActualizacion(CancellationToken cancellationToken = default);
}