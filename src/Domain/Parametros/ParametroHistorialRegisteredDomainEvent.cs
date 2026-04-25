using SharedKernel;

namespace Domain.Parametros;

public sealed record ParametroHistorialRegisteredDomainEvent(Guid HistorialId) : IDomainEvent;