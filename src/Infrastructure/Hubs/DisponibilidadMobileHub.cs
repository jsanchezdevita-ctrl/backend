using Application.Abstractions.Messaging;
using Application.Zonas.GetZonasEstadoMobile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hubs;

[Authorize]
public class DisponibilidadMobileHub : Hub
{
    private readonly ILogger<DisponibilidadMobileHub> _logger;
    private readonly IQueryHandler<GetZonasEstadoMobileQuery, List<ZonaEstadoMobileResponse>> _queryHandler;

    public DisponibilidadMobileHub(
        ILogger<DisponibilidadMobileHub> logger,
        IQueryHandler<GetZonasEstadoMobileQuery, List<ZonaEstadoMobileResponse>> queryHandler)
    {
        _logger = logger;
        _queryHandler = queryHandler;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.User?.Identity?.Name ?? "anon";

        _logger.LogInformation("Cliente mobile conectado: {ConnectionId}, Usuario: {UserId}",
            connectionId, userId);

        await Groups.AddToGroupAsync(connectionId, "disponibilidad-mobile");

        // Obtener userId y rolId de los query strings
        var usuarioId = Context.GetHttpContext()?.Request.Query["UsuarioId"].ToString();
        var rolId = Context.GetHttpContext()?.Request.Query["RolId"].ToString();

        // Enviar estado actual al conectar
        if (Guid.TryParse(usuarioId, out var usuGuid) && Guid.TryParse(rolId, out var rolGuid))
        {
            await EnviarEstadoZonas(usuGuid, rolGuid);
        }

        // Enviar bienvenida
        await Clients.Caller.SendAsync("Conectado", new
        {
            Message = "Conectado al hub de disponibilidad mobile",
            ConnectionId = connectionId,
            Timestamp = DateTime.UtcNow
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        await Groups.RemoveFromGroupAsync(connectionId, "disponibilidad-mobile");

        if (exception != null)
        {
            _logger.LogWarning(exception, "Cliente mobile desconectado con error: {ConnectionId}",
                connectionId);
        }
        else
        {
            _logger.LogInformation("Cliente mobile desconectado: {ConnectionId}", connectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Método que mobile puede llamar para forzar actualización
    public async Task SolicitarActualizacion()
    {
        var connectionId = Context.ConnectionId;
        _logger.LogDebug("Solicitud de actualización desde mobile: {ConnectionId}", connectionId);

        // Obtener userId y rolId de los query strings
        var usuarioId = Context.GetHttpContext()?.Request.Query["UsuarioId"].ToString();
        var rolId = Context.GetHttpContext()?.Request.Query["RolId"].ToString();

        if (Guid.TryParse(usuarioId, out var usuGuid) && Guid.TryParse(rolId, out var rolGuid))
        {
            await EnviarEstadoZonas(usuGuid, rolGuid);
        }

        await Clients.Caller.SendAsync("ActualizacionSolicitada", new
        {
            Message = "Solicitud recibida",
            Timestamp = DateTime.UtcNow
        });
    }

    // Método privado para enviar estado de zonas
    private async Task EnviarEstadoZonas(Guid usuarioId, Guid rolId)
    {
        var query = new GetZonasEstadoMobileQuery(usuarioId, rolId);
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        if (result.IsSuccess)
        {
            await Clients.Caller.SendAsync("ReceiveZonesStatus", new
            {
                data = result.Value,
                timestamp = DateTime.UtcNow
            });
        }
        else
        {
            _logger.LogWarning("Error obteniendo zonas: {Error}", result.Error);
        }
    }
}