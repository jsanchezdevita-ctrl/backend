using Application.Abstractions.Messaging;

namespace Application.Analytics.AccesosDiaSemana.GetWeeklySummary;

public sealed record GetAccesosDiaSemanaWeeklySummaryQuery()
    : IQuery<List<AccesosDiaSemanaSummaryResponse>>;