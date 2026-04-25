using Application.Abstractions.Messaging;

namespace Application.Dispositivos.GetEstadoDispositivoCompleto;

public sealed record GetEstadoDispositivoCompletoQuery(Guid DispositivoId) : IQuery<DispositivoCompletoResponse>;