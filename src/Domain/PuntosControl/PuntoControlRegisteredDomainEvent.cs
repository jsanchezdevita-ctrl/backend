using SharedKernel;

namespace Domain.PuntosControl;

public sealed record PuntoControlRegisteredDomainEvent(Guid PuntoId) : IDomainEvent;
