using Application.Abstractions.Messaging;

namespace Application.Zonas.GetZonasEstadoMobile;

public sealed record GetZonasEstadoMobileQuery(
    Guid UsuarioId,
    Guid RolId) : IQuery<List<ZonaEstadoMobileResponse>>;