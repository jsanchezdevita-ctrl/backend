using SharedKernel;

namespace Domain.Usuarios;

public sealed record UsuarioDeletedDomainEvent(Guid UsuarioId, string Email) : IDomainEvent;