using Application.Abstractions.Messaging;
using Application.Zonas.GetZonasEstadoWeb;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Hubs;

public class DisponibilidadHub : Hub
{
    private static readonly ConcurrentDictionary<string, Guid?> _filtrosPorConexion = new();

    private readonly ILogger<DisponibilidadHub> _logger;
    private readonly IQueryHandler<GetZonasEstadoWebQuery, List<ZonaEstadoWebResponse>> _queryHandler;

    public DisponibilidadHub(
        ILogger<DisponibilidadHub> logger,
        IQueryHandler<GetZonasEstadoWebQuery, List<ZonaEstadoWebResponse>> queryHandler)
    {
        _logger = logger;
        _queryHandler = queryHandler;
    }

    public static List<string> ObtenerConexionesSinFiltro()
    {
        return _filtrosPorConexion
            .Where(kv => !kv.Value.HasValue)
            .Select(kv => kv.Key)
            .ToList();
    }

    public static List<string> ObtenerConexionesConFiltro(Guid zonaId)
    {
        return _filtrosPorConexion
            .Where(kv => kv.Value == zonaId)
            .Select(kv => kv.Key)
            .ToList();
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        // 👇 Simplificado el log (ya no mostramos usuario)
        _logger.LogInformation("Cliente conectado: {ConnectionId}", connectionId);

        await Groups.AddToGroupAsync(connectionId, "disponibilidad");
        _filtrosPorConexion[connectionId] = null;

        await Clients.Caller.SendAsync("Conectado", new
        {
            Message = "Conectado al hub de disponibilidad",
            ConnectionId = connectionId,
            Timestamp = DateTime.UtcNow
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _filtrosPorConexion.TryRemove(connectionId, out _);
        await Groups.RemoveFromGroupAsync(connectionId, "disponibilidad");

        if (exception != null)
        {
            _logger.LogWarning(exception, "Cliente desconectado con error: {ConnectionId}",
                connectionId);
        }
        else
        {
            _logger.LogInformation("Cliente desconectado: {ConnectionId}", connectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task EstablecerFiltro(Guid? zonaId)
    {
        var connectionId = Context.ConnectionId;
        _filtrosPorConexion[connectionId] = zonaId;

        _logger.LogDebug("Filtro establecido para {ConnectionId}: {ZonaId}",
            connectionId, zonaId);

        var query = new GetZonasEstadoWebQuery(zonaId);
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("ReceiveZonesStatus", new
            {
                data = result.Value,
                timestamp = DateTime.UtcNow
            });
        }
    }

    public async Task SolicitarActualizacion()
    {
        var connectionId = Context.ConnectionId;
        _logger.LogDebug("Solicitud de actualización desde: {ConnectionId}", connectionId);

        var zonaId = _filtrosPorConexion.GetValueOrDefault(connectionId);
        var query = new GetZonasEstadoWebQuery(zonaId);
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("ReceiveZonesStatus", new
            {
                data = result.Value,
                timestamp = DateTime.UtcNow
            });
        }

        await Clients.Caller.SendAsync("ActualizacionSolicitada", new
        {
            Message = "Solicitud recibida",
            Timestamp = DateTime.UtcNow
        });
    }
}