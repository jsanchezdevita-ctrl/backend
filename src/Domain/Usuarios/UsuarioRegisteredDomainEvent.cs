using SharedKernel;

namespace Domain.Usuarios;

public sealed record UsuarioRegisteredDomainEvent(Guid UsuarioId) : IDomainEvent;
