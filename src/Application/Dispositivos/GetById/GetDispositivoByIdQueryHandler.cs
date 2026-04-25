using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.GetById;

internal sealed class GetDispositivoByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDispositivoByIdQuery, DispositivoResponse>
{
    public async Task<Result<DispositivoResponse>> Handle(
        GetDispositivoByIdQuery query,
        CancellationToken cancellationToken)
    {
        var dispositivo = await context.Dispositivos
            .Where(d => d.Id == query.DispositivoId && !d.Deleted)
            .Select(d => new
            {
                Dispositivo = d,
                Configuracion = context.DispositivoConfiguraciones
                    .Where(dc => dc.DispositivoId == d.Id)
                    .Select(dc => new DispositivoConfiguracionResponse(
                        dc.FrecuenciaSincronizacionSegundos,
                        dc.PotenciaTransmision.ToString(),
                        dc.CanalComunicacion))
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dispositivo?.Dispositivo is null)
        {
            return Result.Failure<DispositivoResponse>(DispositivosErrores.NotFound(query.DispositivoId));
        }

        return new DispositivoResponse(
            dispositivo.Dispositivo.Id,
            dispositivo.Dispositivo.DispositivoId,
            dispositivo.Dispositivo.Nombre,
            dispositivo.Dispositivo.DireccionIp,
            dispositivo.Dispositivo.UltimaConexion,
            dispositivo.Dispositivo.PuntoControlId,
            dispositivo.Configuracion);
    }
}