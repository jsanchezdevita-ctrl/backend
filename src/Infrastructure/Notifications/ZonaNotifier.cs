using Application.Abstractions.Notifications;
using Application.Zonas.GetZonasEstadoMobile;
using Infrastructure.Hubs; 
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notifications;

internal sealed class ZonaNotifier : IZonaNotifier
{
    private readonly IHubContext<DisponibilidadMobileHub> _hubContext;

    public ZonaNotifier(IHubContext<DisponibilidadMobileHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotificarCambioZonas(Guid rolId, Guid usuarioId, List<ZonaEstadoMobileResponse> zonas, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group("disponibilidad-mobile").SendAsync(
            "ReceiveZonesStatus",
            new { data = zonas, timestamp = DateTime.UtcNow },
            cancellationToken);
    }
}