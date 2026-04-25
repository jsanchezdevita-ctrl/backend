using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics.AccesosDiaSemana.GetWeeklySummary;

internal sealed class GetAccesosDiaSemanaWeeklySummaryQueryHandler(IAnalyticsDbContext context)
    : IQueryHandler<GetAccesosDiaSemanaWeeklySummaryQuery, List<AccesosDiaSemanaSummaryResponse>>
{
    public async Task<Result<List<AccesosDiaSemanaSummaryResponse>>> Handle(
        GetAccesosDiaSemanaWeeklySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var (startOfWeek, endOfWeek) = DateTimeExtensions.GetCurrentWeekRange();

        var startOfWeekUnspecified = DateTime.SpecifyKind(startOfWeek, DateTimeKind.Unspecified);
        var endOfWeekUnspecified = DateTime.SpecifyKind(endOfWeek, DateTimeKind.Unspecified);

        var resumenData = await context.AccesosDiaSemana
            .Where(a => a.Fecha >= startOfWeekUnspecified && a.Fecha <= endOfWeekUnspecified)
            .GroupBy(a => new { a.DiaSemanaCompleto, a.DiaSemanaCorto })
            .Select(g => new
            {
                g.Key.DiaSemanaCompleto,
                g.Key.DiaSemanaCorto,
                Total = g.Sum(x => x.Cantidad)
            })
            .OrderBy(r => r.DiaSemanaCompleto)
            .ToListAsync(cancellationToken);

        var resumen = resumenData
            .Select(r => new AccesosDiaSemanaSummaryResponse(
                r.DiaSemanaCompleto,
                r.DiaSemanaCorto,
                r.Total))
            .ToList();

        return resumen;
    }
}