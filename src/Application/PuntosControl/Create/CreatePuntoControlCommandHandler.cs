using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.PuntosControl.Create;

internal sealed class CreatePuntoControlCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreatePuntoControlCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreatePuntoControlCommand command,
        CancellationToken cancellationToken)
    {
        if (await context.PuntosControl.AnyAsync(
            p => p.Nombre == command.Nombre, cancellationToken))
        {
            return Result.Failure<Guid>(PuntoControlErrors.NombreNotUnique);
        }

        var puntoControl = new PuntoControl
        {
            Id = Guid.NewGuid(),
            Nombre = command.Nombre,
            Ubicacion = command.Ubicacion,
            Tipo = command.Tipo,
            Estado = PuntoControlState.Activo,
            Descripcion = command.Descripcion
        };

        context.PuntosControl.Add(puntoControl);
        await context.SaveChangesAsync(cancellationToken);

        return puntoControl.Id;
    }
}