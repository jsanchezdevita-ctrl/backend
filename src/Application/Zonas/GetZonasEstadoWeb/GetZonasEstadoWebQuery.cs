using Application.Abstractions.Messaging;

namespace Application.Zonas.GetZonasEstadoWeb;

public sealed record GetZonasEstadoWebQuery(
    Guid? ZonaId = null) : IQuery<List<ZonaEstadoWebResponse>>;