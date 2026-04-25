using SharedKernel;

namespace Domain.Zonas;

public sealed record ZonaCreatedDomainEvent(Guid ZonaId) : IDomainEvent;