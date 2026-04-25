using Application.Abstractions.Messaging;

namespace Application.Analytics.AccesosIncidencias.GetDailySummary;

public sealed record GetAccesosIncidenciasDailySummaryQuery()
    : IQuery<List<AccesoIncidenciaDailyResponse>>;