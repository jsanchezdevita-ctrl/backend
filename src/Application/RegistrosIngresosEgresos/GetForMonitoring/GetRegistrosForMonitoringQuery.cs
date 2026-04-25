using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.RegistrosIngresosEgresos.GetForMonitoring;

public sealed record GetRegistrosForMonitoringQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    Guid? EstadoRegistroId = null,
    Guid? TipoUsuarioId = null,
    Guid? PuntoAccesoId = null,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null) : IQuery<PagedResponse<RegistroIngresoEgresoMonitoringResponse>>;