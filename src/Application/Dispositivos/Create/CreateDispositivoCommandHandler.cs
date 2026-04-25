using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Domain.Enums;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.Create;

internal sealed class CreateDispositivoCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateDispositivoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateDispositivoCommand command,
        CancellationToken cancellationToken)
    {
        // Validaciones existentes...
        if (await context.Dispositivos.AnyAsync(
            d => d.DispositivoId == command.DispositivoId, cancellationToken))
        {
            return Result.Failure<Guid>(DispositivosErrores.DispositivoIdNoUnico);
        }

        if (await context.Dispositivos.AnyAsync(
            d => d.Nombre == command.Nombre, cancellationToken))
        {
            return Result.Failure<Guid>(DispositivosErrores.NombreNoUnico);
        }

        if (await context.Dispositivos.AnyAsync(
            d => d.DireccionIp == command.DireccionIp, cancellationToken))
        {
            return Result.Failure<Guid>(DispositivosErrores.DireccionIpNoUnica);
        }

        // Validar que el PuntoControl exista
        var puntoControlExiste = await context.PuntosControl
            .AnyAsync(pc => pc.Id == command.PuntoControlId, cancellationToken);

        if (!puntoControlExiste)
        {
            return Result.Failure<Guid>(PuntoControlErrors.NotFound(command.PuntoControlId));
        }

        var puntoControlEnUso = await context.Dispositivos
            .AnyAsync(d => d.PuntoControlId == command.PuntoControlId, cancellationToken);

        if (puntoControlEnUso)
        {
            return Result.Failure<Guid>(DispositivosErrores.PuntoControlYaEnUso);
        }

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            DispositivoId = command.DispositivoId,
            Nombre = command.Nombre,
            DireccionIp = command.DireccionIp,
            PuntoControlId = command.PuntoControlId
        };

        var configuracion = new DispositivoConfiguracion
        {
            Id = Guid.NewGuid(),
            DispositivoId = dispositivo.Id,
            FrecuenciaSincronizacionSegundos = 30,
            PotenciaTransmision = DispositivoPowerTransmission.Media,
            CanalComunicacion = 1
        };

        dispositivo.Raise(new DispositivoRegisteredDomainEvent(dispositivo.Id));

        context.Dispositivos.Add(dispositivo);
        context.DispositivoConfiguraciones.Add(configuracion);

        await context.SaveChangesAsync(cancellationToken);

        return dispositivo.Id;
    }
}