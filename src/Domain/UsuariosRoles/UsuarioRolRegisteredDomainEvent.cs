using SharedKernel;

namespace Domain.UsuariosRoles;

public sealed record UsuarioRolRegisteredDomainEvent(Guid UsuarioId, Guid RolId) : IDomainEvent;
