using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics.AccesosPorHora.GetDailySummary;

internal sealed class GetAccesosPorHoraDailySummaryQueryHandler(IAnalyticsDbContext context)
    : IQueryHandler<GetAccesosPorHoraDailySummaryQuery, List<AccesosPorHoraDailyResponse>>
{
    public async Task<Result<List<AccesosPorHoraDailyResponse>>> Handle(
        GetAccesosPorHoraDailySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Now.Date;

        var resumenData = await context.AccesosPorHora
            .Where(a => a.Fecha == today) // ← Ahora ambos son DateTimeKind.Unspecified
            .OrderBy(a => a.Hora)
            .Select(a => new AccesosPorHoraDailyResponse(a.Hora, a.Cantidad))
            .ToListAsync(cancellationToken);

        return resumenData;
    }
}