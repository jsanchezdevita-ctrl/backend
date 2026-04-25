using SharedKernel;

namespace Domain.Roles;

public sealed record RolRegisteredDomainEvent(Guid RolId) : IDomainEvent;
