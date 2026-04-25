using Application.Abstractions.Messaging;

namespace Application.Zonas.Create;

public sealed record CreateZonaCommand(
    string Nombre,
    string Descripcion,
    List<ZonaRolRequest> Roles,
    List<Guid> PuntoControlIds)
    : ICommand<Guid>;

public sealed record ZonaRolRequest(
    Guid RolId,
    int CapacidadMaxima);