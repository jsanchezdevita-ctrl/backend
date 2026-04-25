using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.Delete;

internal sealed class DeleteUsuarioCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<DeleteUsuarioCommand>
{
    public async Task<Result> Handle(DeleteUsuarioCommand command, CancellationToken cancellationToken)
    {
        if (command.UsuarioId == userContext.UsuarioId)
        {
            return Result.Failure(UsuarioErrors.CannotDeleteSelf);
        }

        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == command.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            return Result.Failure(UsuarioErrors.NotFound(command.UsuarioId));
        }

        bool isAdmin = await context.UsuariosRoles
            .Where(ur => ur.UsuarioId == command.UsuarioId)
            .Join(context.Roles,
                ur => ur.RolId,
                r => r.Id,
                (ur, r) => r.EsAdmin)
            .AnyAsync(esAdmin => esAdmin, cancellationToken);

        if (isAdmin)
        {
            return Result.Failure(UsuarioErrors.CannotDeleteAdminUsers);
        }

        usuario.SoftDelete(userContext.Email);

        usuario.Raise(new UsuarioDeletedDomainEvent(usuario.Id, usuario.Email));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}