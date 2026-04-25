using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.Update;

internal sealed class UpdateDispositivoCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateDispositivoCommand>
{
    public async Task<Result> Handle(
        UpdateDispositivoCommand command,
        CancellationToken cancellationToken)
    {
        var dispositivo = await context.Dispositivos
            .FirstOrDefaultAsync(d => d.Id == command.DispositivoId, cancellationToken);

        if (dispositivo is null)
        {
            return Result.Failure(DispositivosErrores.NotFound(command.DispositivoId));
        }

        if (await context.Dispositivos.AnyAsync(
            d => d.DispositivoId == command.DispositivoIdCodigo && d.Id != command.DispositivoId, cancellationToken))
        {
            return Result.Failure(DispositivosErrores.DispositivoIdNoUnico);
        }

        if (await context.Dispositivos.AnyAsync(
            d => d.Nombre == command.Nombre && d.Id != command.DispositivoId, cancellationToken))
        {
            return Result.Failure(DispositivosErrores.NombreNoUnico);
        }

        if (await context.Dispositivos.AnyAsync(
            d => d.DireccionIp == command.DireccionIp && d.Id != command.DispositivoId, cancellationToken))
        {
            return Result.Failure(DispositivosErrores.DireccionIpNoUnica);
        }

        var puntoControlExiste = await context.PuntosControl
            .AnyAsync(pc => pc.Id == command.PuntoControlId, cancellationToken);

        if (!puntoControlExiste)
        {
            return Result.Failure(PuntoControlErrors.NotFound(command.PuntoControlId));
        }

        dispositivo.DispositivoId = command.DispositivoIdCodigo;
        dispositivo.Nombre = command.Nombre;
        dispositivo.DireccionIp = command.DireccionIp;
        dispositivo.PuntoControlId = command.PuntoControlId;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}