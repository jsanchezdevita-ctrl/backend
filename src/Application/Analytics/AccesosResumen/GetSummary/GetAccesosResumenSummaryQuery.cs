using Application.Abstractions.Messaging;

namespace Application.Analytics.AccesosResumen.GetSummary;

public sealed record GetAccesosResumenSummaryQuery()
    : IQuery<AccesosResumenSummaryResponse>;