using Application.Abstractions.Notifications;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notifications;

internal sealed class MonitoreoNotifier : IMonitoreoNotifier
{
    private readonly IHubContext<MonitoreoHub> _hubContext;

    public MonitoreoNotifier(IHubContext<MonitoreoHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotificarActualizacion(CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.All.SendAsync(
            "AvisoActualizacion",
            cancellationToken: cancellationToken);
    }
}