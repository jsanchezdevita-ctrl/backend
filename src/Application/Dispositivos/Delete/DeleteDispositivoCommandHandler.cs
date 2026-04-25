using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.Delete;

internal sealed class DeleteDispositivoCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<DeleteDispositivoCommand>
{
    public async Task<Result> Handle(
        DeleteDispositivoCommand command,
        CancellationToken cancellationToken)
    {
        var dispositivo = await context.Dispositivos
            .FirstOrDefaultAsync(d => d.Id == command.DispositivoId, cancellationToken);

        if (dispositivo is null)
        {
            return Result.Failure(DispositivosErrores.NotFound(command.DispositivoId));
        }

        dispositivo.SoftDelete(userContext.Email);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}