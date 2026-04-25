using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Zonas.GetAll;

public sealed record GetAllZonasQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<ZonaResponse>>;

public record ZonaResponse(
    Guid Id,
    string Nombre,
    string Descripcion,
    List<ZonaRolInfo> Roles,
    List<ZonaPuntoControlInfo> PuntosControl);

public record ZonaRolInfo(
    Guid Id,
    Guid RolId,
    string RolNombre,
    int CapacidadMaxima,
    int EspacioUtilizado,
    int EspacioDisponible);

public record ZonaPuntoControlInfo(
    Guid Id,
    Guid PuntoControlId,
    string PuntoControlNombre,
    string Ubicacion);