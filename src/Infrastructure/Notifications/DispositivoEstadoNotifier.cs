using Application.Abstractions.Notifications;
using Application.Dispositivos.GetEstadoDispositivoCompleto;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notifications;

internal sealed class DispositivoEstadoNotifier : IDispositivoEstadoNotifier
{
    private readonly IHubContext<DispositivoEstadoHub> _hubContext;

    public DispositivoEstadoNotifier(IHubContext<DispositivoEstadoHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotificarCambioDispositivo(Guid dispositivoId, DispositivoCompletoResponse data, CancellationToken cancellationToken = default)
    {
        var conexiones = DispositivoEstadoHub.ObtenerConexionesPorDispositivo(dispositivoId);

        foreach (var connectionId in conexiones)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync(
                "ReceiveEstadoDispositivo",
                new { data, timestamp = DateTime.UtcNow },
                cancellationToken);
        }
    }
}