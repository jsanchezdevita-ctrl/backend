using Application.Abstractions.Realtime;
using Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Web.Api.Realtime;

public class DisponibilidadNotifier : IDisponibilidadNotifier
{
    private readonly IHubContext<DisponibilidadHub> _hubContext;

    public DisponibilidadNotifier(IHubContext<DisponibilidadHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyDisponibilidadActualizada(DisponibilidadPorRolesResponse data)
    {
        await _hubContext.Clients.All.SendAsync("DisponibilidadActualizada", data);
    }
}