using SharedKernel;

namespace Domain.EstadosRegistro;

public sealed record EstadoRegistroRegisteredDomainEvent(Guid EstadoRegistroId) : IDomainEvent;