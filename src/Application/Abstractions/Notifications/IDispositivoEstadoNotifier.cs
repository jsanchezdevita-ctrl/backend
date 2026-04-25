using Application.Dispositivos.GetEstadoDispositivoCompleto;

namespace Application.Abstractions.Notifications;

public interface IDispositivoEstadoNotifier
{
    Task NotificarCambioDispositivo(Guid dispositivoId, DispositivoCompletoResponse data, CancellationToken cancellationToken = default);
}