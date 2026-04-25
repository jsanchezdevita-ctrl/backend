using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Hubs;

public class MonitoreoHub : Hub
{
    private static readonly ConcurrentDictionary<string, bool> _conexionesActivas = new();
    private readonly ILogger<MonitoreoHub> _logger;

    public MonitoreoHub(ILogger<MonitoreoHub> logger)
    {
        _logger = logger;
    }

    public static List<string> ObtenerTodasLasConexiones()
    {
        return _conexionesActivas.Keys.ToList();
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        _conexionesActivas[connectionId] = true;

        _logger.LogInformation("Cliente conectado a MonitoreoHub: {ConnectionId}", connectionId);

        await Clients.Caller.SendAsync("Conectado", new
        {
            Message = "Conectado al hub de monitoreo",
            ConnectionId = connectionId,
            Timestamp = DateTime.UtcNow
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        _conexionesActivas.TryRemove(connectionId, out _);

        if (exception != null)
        {
            _logger.LogWarning(exception, "Cliente desconectado de MonitoreoHub con error: {ConnectionId}", connectionId);
        }
        else
        {
            _logger.LogInformation("Cliente desconectado de MonitoreoHub: {ConnectionId}", connectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }


    public async Task NotificarActualizacionATodos(string mensaje = "actualizar")
    {
        _logger.LogInformation("Notificando a {Count} clientes conectados", _conexionesActivas.Count);

        await Clients.All.SendAsync("AvisoActualizacion", new
        {
            Mensaje = mensaje,
            Timestamp = DateTime.UtcNow,
            TotalClientes = _conexionesActivas.Count
        });
    }
}