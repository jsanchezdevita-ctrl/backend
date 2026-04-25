using Application.Abstractions.Notifications;
using Application.Zonas.GetZonasEstadoWeb;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Notifications;

internal sealed class ZonaNotifierWeb : IZonaNotifierWeb
{
    private readonly IHubContext<DisponibilidadHub> _hubContext;
    private readonly ILogger<ZonaNotifierWeb> _logger;

    public ZonaNotifierWeb(
        IHubContext<DisponibilidadHub> hubContext,
        ILogger<ZonaNotifierWeb> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotificarCambioZonas(Guid zonaIdModificada, List<ZonaEstadoWebResponse> todasLasZonas, CancellationToken cancellationToken = default)
    {
        var conexionesSinFiltro = DisponibilidadHub.ObtenerConexionesSinFiltro();
        var conexionesConFiltroCoincidente = DisponibilidadHub.ObtenerConexionesConFiltro(zonaIdModificada);

        if (conexionesSinFiltro.Any())
        {
            _logger.LogDebug("Enviando todas las zonas a {Count} conexiones sin filtro",
                conexionesSinFiltro.Count);

            await _hubContext.Clients.Clients(conexionesSinFiltro).SendAsync(
                "ReceiveZonesStatus",
                new { data = todasLasZonas, timestamp = DateTime.UtcNow },
                cancellationToken);
        }

        if (conexionesConFiltroCoincidente.Any())
        {
            var zonaModificada = todasLasZonas.First(z => z.ZonaId == zonaIdModificada);

            _logger.LogDebug("Enviando zona {ZonaId} a {Count} conexiones con filtro",
                zonaIdModificada, conexionesConFiltroCoincidente.Count);

            await _hubContext.Clients.Clients(conexionesConFiltroCoincidente).SendAsync(
                "ReceiveZonesStatus",
                new
                {
                    data = new List<ZonaEstadoWebResponse> { zonaModificada },
                    timestamp = DateTime.UtcNow
                },
                cancellationToken);
        }
    }
}