using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.GetById;

internal sealed class GetZonaByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetZonaByIdQuery, ZonaResponse>
{
    public async Task<Result<ZonaResponse>> Handle(
        GetZonaByIdQuery query,
        CancellationToken cancellationToken)
    {
        var zona = await context.Zonas
            .Where(z => z.Id == query.ZonaId && !z.Deleted)
            .Select(z => new
            {
                Zona = z,

                Roles = context.ZonasRoles
                    .Where(zr => zr.ZonaId == z.Id)
                    .Join(context.Roles,
                        zr => zr.RolId,
                        r => r.Id,
                        (zr, r) => new ZonaRolInfo(
                            zr.Id,
                            r.Id,
                            r.NombreRol,
                            zr.CapacidadMaxima,
                            zr.EspacioUtilizado,
                            zr.CapacidadMaxima - zr.EspacioUtilizado))
                    .ToList(),

                PuntosControl = context.ZonasPuntosControl
                    .Where(zpc => zpc.ZonaId == z.Id)
                    .Join(context.PuntosControl,
                        zpc => zpc.PuntoControlId,
                        pc => pc.Id,
                        (zpc, pc) => new ZonaPuntoControlInfo(
                            zpc.Id,
                            pc.Id,
                            pc.Nombre,
                            pc.Ubicacion))
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (zona?.Zona is null)
        {
            return Result.Failure<ZonaResponse>(ZonaErrors.NotFound(query.ZonaId));
        }

        return new ZonaResponse(
            zona.Zona.Id,
            zona.Zona.Nombre,
            zona.Zona.Descripcion,
            zona.Roles,
            zona.PuntosControl);
    }
}