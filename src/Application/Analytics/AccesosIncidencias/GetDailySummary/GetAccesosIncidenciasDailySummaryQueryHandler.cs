using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics.AccesosIncidencias.GetDailySummary;

internal sealed class GetAccesosIncidenciasDailySummaryQueryHandler(IAnalyticsDbContext context)
    : IQueryHandler<GetAccesosIncidenciasDailySummaryQuery, List<AccesoIncidenciaDailyResponse>>
{
    public async Task<Result<List<AccesoIncidenciaDailyResponse>>> Handle(
        GetAccesosIncidenciasDailySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Now.Date;

        var datosPorPuntoControl = await context.AccesosIncidencias
            .Where(a => a.Fecha == today)
            .GroupBy(a => new { a.NombrePuntoControl, a.Fecha })
            .Select(g => new
            {
                g.Key.NombrePuntoControl,
                g.Key.Fecha,
                Datos = g.ToList()
            })
            .OrderBy(r => r.NombrePuntoControl)
            .ToListAsync(cancellationToken);

        var resultado = datosPorPuntoControl
            .Select(g => new AccesoIncidenciaDailyResponse(
                g.NombrePuntoControl,
                g.Fecha,
                g.Datos
                    .GroupBy(x => x.Incidencia)
                    .Select(ig => new IncidenciaResponse(
                        ig.Key,
                        ig.Sum(x => x.Cantidad)
                    ))
                    .OrderByDescending(i => i.Cantidad)
                    .ToList()
            ))
            .ToList();

        return resultado;
    }
}