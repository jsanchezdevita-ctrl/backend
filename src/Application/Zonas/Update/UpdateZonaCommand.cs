using Application.Abstractions.Messaging;

namespace Application.Zonas.Update;

public sealed record UpdateZonaCommand(
    Guid ZonaId,
    string Nombre,
    string Descripcion,
    List<ZonaRolRequest> Roles,
    List<Guid> PuntoControlIds)
    : ICommand;


public sealed record ZonaRolRequest(
    Guid RolId,
    int CapacidadMaxima);