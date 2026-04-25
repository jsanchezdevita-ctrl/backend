using SharedKernel;

namespace Domain.Usuarios;

public sealed record UsuarioUpdateDomainEvent(Guid UsuarioId) : IDomainEvent;