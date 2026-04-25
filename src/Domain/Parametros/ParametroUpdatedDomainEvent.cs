using SharedKernel;

namespace Domain.Parametros;

public sealed record ParametroUpdatedDomainEvent(Guid ParametroId, string Llave) : IDomainEvent;