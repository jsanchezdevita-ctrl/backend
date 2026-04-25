using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.PuntosControl.Update;

internal sealed class UpdatePuntoControlCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdatePuntoControlCommand>
{
    public async Task<Result> Handle(
        UpdatePuntoControlCommand command,
        CancellationToken cancellationToken)
    {
        var puntoControl = await context.PuntosControl
            .FirstOrDefaultAsync(p => p.Id == command.PuntoControlId, cancellationToken);

        if (puntoControl is null)
        {
            return Result.Failure(PuntoControlErrors.NotFound(command.PuntoControlId));
        }

        if (await context.PuntosControl.AnyAsync(
            p => p.Nombre == command.Nombre && p.Id != command.PuntoControlId, cancellationToken))
        {
            return Result.Failure(PuntoControlErrors.NombreNotUnique);
        }

        puntoControl.Nombre = command.Nombre;
        puntoControl.Ubicacion = command.Ubicacion;
        puntoControl.Tipo = command.Tipo;
        puntoControl.Estado = command.Estado;
        puntoControl.Descripcion = command.Descripcion;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}