using Application.Abstractions.Messaging;

namespace Application.Zonas.GetById;

public sealed record GetZonaByIdQuery(Guid ZonaId) : IQuery<ZonaResponse>;

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