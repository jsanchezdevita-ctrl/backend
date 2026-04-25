using SharedKernel;

namespace Domain.Parametros;

public sealed record ParametroRegisteredDomainEvent(Guid ParametroId) : IDomainEvent;