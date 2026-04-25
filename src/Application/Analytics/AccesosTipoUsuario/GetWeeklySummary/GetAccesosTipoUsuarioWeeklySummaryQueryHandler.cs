using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics.AccesosTipoUsuario.GetWeeklySummary;

internal sealed class GetAccesosTipoUsuarioWeeklySummaryQueryHandler(IAnalyticsDbContext context)
    : IQueryHandler<GetAccesosTipoUsuarioWeeklySummaryQuery, List<AccesosTipoUsuarioSummaryResponse>>
{
    public async Task<Result<List<AccesosTipoUsuarioSummaryResponse>>> Handle(
        GetAccesosTipoUsuarioWeeklySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var (startOfWeek, endOfWeek) = DateTimeExtensions.GetCurrentWeekRange();

        var startOfWeekUnspecified = DateTime.SpecifyKind(startOfWeek, DateTimeKind.Unspecified);
        var endOfWeekUnspecified = DateTime.SpecifyKind(endOfWeek, DateTimeKind.Unspecified);

        var resumenData = await context.AccesosTipoUsuario
            .Where(a => a.Fecha >= startOfWeekUnspecified && a.Fecha <= endOfWeekUnspecified)
            .GroupBy(a => a.TipoUsuario)
            .Select(g => new
            {
                TipoUsuario = g.Key,
                Total = g.Sum(x => x.Cantidad)
            })
            .OrderByDescending(r => r.Total)
            .ToListAsync(cancellationToken);

        var resumen = resumenData
            .Select(r => new AccesosTipoUsuarioSummaryResponse(r.TipoUsuario, r.Total))
            .ToList();

        return resumen;
    }
}