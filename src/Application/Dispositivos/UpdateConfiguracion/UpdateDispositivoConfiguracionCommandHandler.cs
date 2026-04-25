using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.UpdateConfiguracion;

internal sealed class UpdateDispositivoConfiguracionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateDispositivoConfiguracionCommand>
{
    public async Task<Result> Handle(
        UpdateDispositivoConfiguracionCommand command,
        CancellationToken cancellationToken)
    {
        var dispositivoExiste = await context.Dispositivos
            .AnyAsync(d => d.Id == command.DispositivoId, cancellationToken);

        if (!dispositivoExiste)
        {
            return Result.Failure(DispositivosErrores.NotFound(command.DispositivoId));
        }

        var configuracion = await context.DispositivoConfiguraciones
            .FirstOrDefaultAsync(dc => dc.DispositivoId == command.DispositivoId, cancellationToken);

        if (configuracion is null)
        {
            configuracion = new DispositivoConfiguracion
            {
                Id = Guid.NewGuid(),
                DispositivoId = command.DispositivoId,
                FrecuenciaSincronizacionSegundos = command.FrecuenciaSincronizacionSegundos,
                PotenciaTransmision = command.PotenciaTransmision,
                CanalComunicacion = command.CanalComunicacion
            };

            context.DispositivoConfiguraciones.Add(configuracion);
        }
        else
        {
            configuracion.FrecuenciaSincronizacionSegundos = command.FrecuenciaSincronizacionSegundos;
            configuracion.PotenciaTransmision = command.PotenciaTransmision;
            configuracion.CanalComunicacion = command.CanalComunicacion;
        }

        configuracion.Raise(new DispositivoConfiguracionUpdatedDomainEvent(command.DispositivoId));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}