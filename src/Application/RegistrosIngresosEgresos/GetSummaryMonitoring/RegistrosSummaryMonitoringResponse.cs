namespace Application.RegistrosIngresosEgresos.GetSummaryMonitoring;

public sealed record RegistrosSummaryMonitoringResponse(
    MetricInfo Ultimos5Min,
    MetricInfo Total,
    MetricInfo Autorizados,
    MetricInfo Rechazados);

public sealed record MetricInfo(string Id, int Count);