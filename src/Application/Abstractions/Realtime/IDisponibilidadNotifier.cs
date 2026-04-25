using Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;

namespace Application.Abstractions.Realtime;

public interface IDisponibilidadNotifier
{
    Task NotifyDisponibilidadActualizada(DisponibilidadPorRolesResponse data);
}
