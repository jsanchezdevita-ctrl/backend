using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.PuntosControl.Delete;

internal sealed class DeletePuntoControlCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<DeletePuntoControlCommand>
{
    public async Task<Result> Handle(
        DeletePuntoControlCommand command,
        CancellationToken cancellationToken
        )
    {
        var puntoControl = await context.PuntosControl
            .FirstOrDefaultAsync(p => p.Id == command.PuntoControlId, cancellationToken);

        if (puntoControl is null)
        {
            return Result.Failure(PuntoControlErrors.NotFound(command.PuntoControlId));
        }

        if (puntoControl.Estado == PuntoControlState.Activo)
        {
            return Result.Failure(PuntoControlErrors.CannotDeleteActive(command.PuntoControlId));
        }

        puntoControl.SoftDelete(userContext.Email);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}