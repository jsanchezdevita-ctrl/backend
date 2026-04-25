using Application.Abstractions.Messaging;

namespace Application.RegistrosIngresosEgresos.GetSummaryMonitoring;

public sealed record GetRegistrosSummaryMonitoringQuery()
    : IQuery<RegistrosSummaryMonitoringResponse>;

