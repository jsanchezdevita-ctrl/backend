using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.GetZonasEstadoMobile;

internal sealed class GetZonasEstadoMobileQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetZonasEstadoMobileQuery, List<ZonaEstadoMobileResponse>>
{
    public async Task<Result<List<ZonaEstadoMobileResponse>>> Handle(
        GetZonasEstadoMobileQuery query,
        CancellationToken cancellationToken)
    {
        var zonas = await context.ZonasRoles
            .Where(zr => zr.RolId == query.RolId && !zr.Deleted)
            .Join(context.Zonas.Where(z => !z.Deleted),
                zr => zr.ZonaId,
                z => z.Id,
                (zr, z) => new ZonaEstadoMobileResponse(
                    z.Id,
                    z.Nombre,
                    zr.CapacidadMaxima,
                    zr.EspacioUtilizado
                ))
            .ToListAsync(cancellationToken);

        return Result.Success(zonas);
    }
}