using Application.Abstractions.Messaging;
using Application.Dispositivos.GetEstadoDispositivoCompleto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Hubs;
// 1. C
public class DispositivoEstadoHub : Hub
{
    private static readonly ConcurrentDictionary<string, Guid> _dispositivosMonitoreados = new();

    private readonly ILogger<DispositivoEstadoHub> _logger;
    private readonly IQueryHandler<GetEstadoDispositivoCompletoQuery, DispositivoCompletoResponse> _queryHandler;

    public DispositivoEstadoHub(
        ILogger<DispositivoEstadoHub> logger,
        IQueryHandler<GetEstadoDispositivoCompletoQuery, DispositivoCompletoResponse> queryHandler)
    {
        _logger = logger;
        _queryHandler = queryHandler;
    }

    public static List<string> ObtenerConexionesPorDispositivo(Guid dispositivoId)
    {
        return _dispositivosMonitoreados
            .Where(kv => kv.Value == dispositivoId)
            .Select(kv => kv.Key)
            .ToList();
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        _logger.LogInformation("Cliente conectado a DispositivoEstadoHub: {ConnectionId}", connectionId);

        await Groups.AddToGroupAsync(connectionId, "dispositivos-estado");

        await Clients.Caller.SendAsync("Conectado", new
        {
            Message = "Conectado al hub de estado de dispositivos",
            ConnectionId = connectionId,
            Timestamp = DateTime.UtcNow
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _dispositivosMonitoreados.TryRemove(connectionId, out _);
        await Groups.RemoveFromGroupAsync(connectionId, "dispositivos-estado");

        if (exception != null)
        {
            _logger.LogWarning(exception, "Cliente desconectado de DispositivoEstadoHub con error: {ConnectionId}", connectionId);
        }
        else
        {
            _logger.LogInformation("Cliente desconectado de DispositivoEstadoHub: {ConnectionId}", connectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task MonitorearDispositivo(Guid dispositivoId)
    {
        var connectionId = Context.ConnectionId;

        _dispositivosMonitoreados[connectionId] = dispositivoId;

        _logger.LogInformation("Cliente {ConnectionId} comenzó a monitorear dispositivo: {DispositivoId}",
            connectionId, dispositivoId);

        await EnviarEstadoDispositivo(connectionId, dispositivoId);
    }

    public async Task SolicitarActualizacion()
    {
        var connectionId = Context.ConnectionId;

        if (_dispositivosMonitoreados.TryGetValue(connectionId, out var dispositivoId))
        {
            _logger.LogDebug("Cliente {ConnectionId} solicitó actualización para dispositivo: {DispositivoId}",
                connectionId, dispositivoId);

            await EnviarEstadoDispositivo(connectionId, dispositivoId);
        }
        else
        {
            _logger.LogWarning("Cliente {ConnectionId} solicitó actualización pero no está monitoreando ningún dispositivo",
                connectionId);

            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Debe llamar a MonitorearDispositivo primero",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    private async Task EnviarEstadoDispositivo(string connectionId, Guid dispositivoId)
    {
        var query = new GetEstadoDispositivoCompletoQuery(dispositivoId);
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        if (result.IsSuccess)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveEstadoDispositivo", new
            {
                data = result.Value,
                timestamp = DateTime.UtcNow
            });

            await Clients.All.SendAsync("ReceiveEstadoDispositivo", new
            {
                data = result.Value,
                timestamp = DateTime.UtcNow
            });


        }
        else
        {
            _logger.LogWarning("Error al obtener estado del dispositivo {DispositivoId}: {Error}",
                dispositivoId, result.Error);

            await Clients.Client(connectionId).SendAsync("Error", new
            {
                Message = result.Error.Description,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}