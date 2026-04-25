using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.GetZonasEstadoWeb;

internal sealed class GetZonasEstadoWebQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetZonasEstadoWebQuery, List<ZonaEstadoWebResponse>>
{
    public async Task<Result<List<ZonaEstadoWebResponse>>> Handle(
        GetZonasEstadoWebQuery query,
        CancellationToken cancellationToken)
    {
        // Primero obtenemos todas las zonas activas
        var zonasQuery = context.Zonas
            .Where(z => !z.Deleted)
            .AsQueryable();

        if (query.ZonaId.HasValue)
        {
            zonasQuery = zonasQuery.Where(z => z.Id == query.ZonaId.Value);
        }

        var zonas = await zonasQuery
            .Select(z => new
            {
                Zona = z,
                // Sumamos CapacidadMaxima y EspacioUtilizado de todos los ZonasRoles activos de esta zona
                Totales = context.ZonasRoles
                    .Where(zr => zr.ZonaId == z.Id && !zr.Deleted)
                    .GroupBy(zr => 1)
                    .Select(g => new
                    {
                        TotalCapacidad = g.Sum(zr => zr.CapacidadMaxima),
                        TotalUtilizado = g.Sum(zr => zr.EspacioUtilizado)
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        var response = zonas.Select(x => new ZonaEstadoWebResponse(
            x.Zona.Id,
            x.Zona.Nombre,
            x.Totales?.TotalCapacidad ?? 0,
            x.Totales?.TotalUtilizado ?? 0
        )).ToList();

        return Result.Success(response);
    }
}