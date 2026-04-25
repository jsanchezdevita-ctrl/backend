using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.RegistrosIngresosEgresos.GetSummaryMonitoring;

internal sealed class GetRegistrosSummaryMonitoringQueryHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetRegistrosSummaryMonitoringQuery, RegistrosSummaryMonitoringResponse>
{
    public async Task<Result<RegistrosSummaryMonitoringResponse>> Handle(
        GetRegistrosSummaryMonitoringQuery query,
        CancellationToken cancellationToken)
    {
        var totalRegistros = await context.RegistrosIngresosEgresos
            .CountAsync(cancellationToken);

        var cincoMinutosAtras = dateTimeProvider.UtcNow.AddMinutes(-5);
        var ultimos5Min = await context.RegistrosIngresosEgresos
            .CountAsync(r => r.Fecha >= cincoMinutosAtras, cancellationToken);

        var conteoPorEstado = await context.RegistrosIngresosEgresos
            .Join(context.EstadosRegistro,
                r => r.EstadoRegistroId,
                er => er.Id,
                (r, er) => new { Registro = r, EstadoRegistro = er })
            .GroupBy(x => x.EstadoRegistro.Descripcion.ToLower())
            .Select(g => new
            {
                Estado = g.Key,
                Cantidad = g.Count()
            })
            .ToListAsync(cancellationToken);

        var autorizados = conteoPorEstado
            .FirstOrDefault(x => x.Estado == "autorizado")?.Cantidad ?? 0;

        var rechazados = conteoPorEstado
            .FirstOrDefault(x => x.Estado == "denegado")?.Cantidad ?? 0;

        return new RegistrosSummaryMonitoringResponse(
            Ultimos5Min: new MetricInfo(Guid.NewGuid().ToString(), ultimos5Min),
            Total: new MetricInfo(Guid.NewGuid().ToString(), totalRegistros),
            Autorizados: new MetricInfo(Guid.NewGuid().ToString(), autorizados),
            Rechazados: new MetricInfo(Guid.NewGuid().ToString(), rechazados));
    }
}