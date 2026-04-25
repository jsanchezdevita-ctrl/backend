using SharedKernel;

namespace Domain.Permisos;

public sealed record PermisoCreatedDomainEvent(Guid PermisoId) : IDomainEvent;
