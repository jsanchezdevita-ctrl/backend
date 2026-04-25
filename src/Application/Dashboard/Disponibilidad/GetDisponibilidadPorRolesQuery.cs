using Application.Abstractions.Messaging;

namespace Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;

public sealed record GetDisponibilidadPorRolesQuery(Guid zonaId) : 
    IQuery<DisponibilidadPorRolesResponse>;

public record DisponibilidadPorRolesResponse(List<DisponibilidadRolSummary> Summary);

public record DisponibilidadRolSummary(
    Guid RolId,
    string RolNombre,
    bool EsAdmin,
    int TotalCapacidad,
    int EspacioUtilizado,
    int EspaciosLibres,
    double PorcentajeOcupado,
    string Estado);