using Domain.Usuarios;
using SharedKernel;

namespace Application.Usuarios.Register;

internal sealed class UsuarioRegisteredDomainEventHandler : IDomainEventHandler<UsuarioRegisteredDomainEvent>
{
    public Task Handle(UsuarioRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}

internal sealed class UserRegisteredDomainEventHandler1 : IDomainEventHandler<UsuarioRegisteredDomainEvent>
{
    public Task Handle(UsuarioRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}
