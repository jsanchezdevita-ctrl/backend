using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.UpdateFcmToken;

internal sealed class UpdateUsuarioFcmTokenCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateUsuarioFcmTokenCommand>
{
    public async Task<Result> Handle(
        UpdateUsuarioFcmTokenCommand command,
        CancellationToken cancellationToken)
    {
        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == userContext.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            return Result.Failure(UsuarioErrors.NotFound(userContext.UsuarioId));
        }

        usuario.FcmToken = command.FcmToken;
        usuario.FcmTokenUpdatedAt = dateTimeProvider.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}