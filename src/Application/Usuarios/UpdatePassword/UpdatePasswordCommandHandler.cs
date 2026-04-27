using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.UpdatePassword;

internal sealed class UpdatePasswordCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdatePasswordCommand>
{
    public async Task<Result> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == command.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            return Result.Failure(UsuarioErrors.NotFound(command.UsuarioId));
        }

        // Hash the new password
        usuario.PasswordHash = passwordHasher.Hash(command.NewPassword);
        usuario.FechaUltimaModificacion = dateTimeProvider.UtcNow;

        // Raise a domain event if needed (e.g., for auditing or notifications)
        // usuario.Raise(new UsuarioPasswordUpdatedDomainEvent(usuario.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
