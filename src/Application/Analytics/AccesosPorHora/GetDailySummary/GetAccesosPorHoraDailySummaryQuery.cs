using Application.Abstractions.Messaging;

namespace Application.Analytics.AccesosPorHora.GetDailySummary;

public sealed record GetAccesosPorHoraDailySummaryQuery()
    : IQuery<List<AccesosPorHoraDailyResponse>>;