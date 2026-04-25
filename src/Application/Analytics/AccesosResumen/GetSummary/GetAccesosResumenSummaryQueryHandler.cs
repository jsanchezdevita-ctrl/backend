using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics.AccesosResumen.GetSummary;

internal sealed class GetAccesosResumenSummaryQueryHandler(IAnalyticsDbContext context)
    : IQueryHandler<GetAccesosResumenSummaryQuery, AccesosResumenSummaryResponse>
{
    public async Task<Result<AccesosResumenSummaryResponse>> Handle(
        GetAccesosResumenSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var totalAccesos = await ObtenerTotalAccesos(cancellationToken);
        var totalIncidencias = await ObtenerTotalIncidencias(cancellationToken);
        var horaPico = await ObtenerHoraPico(cancellationToken);
        var promedioDiario = await ObtenerPromedioDiario(cancellationToken);

        return new AccesosResumenSummaryResponse(
            totalAccesos,
            totalIncidencias,
            horaPico,
            promedioDiario);
    }

    private async Task<AccesosResponse> ObtenerTotalAccesos(CancellationToken cancellationToken)
    {
        var (inicioSemanaActual, finSemanaActual) = DateTimeExtensions.GetCurrentWeekRange();
        var (inicioSemanaAnterior, finSemanaAnterior) = DateTimeExtensions.GetPreviousWeekRange();

        var inicioSemanaActualUnspecified = DateTime.SpecifyKind(inicioSemanaActual, DateTimeKind.Unspecified);
        var finSemanaActualUnspecified = DateTime.SpecifyKind(finSemanaActual, DateTimeKind.Unspecified);
        var inicioSemanaAnteriorUnspecified = DateTime.SpecifyKind(inicioSemanaAnterior, DateTimeKind.Unspecified);
        var finSemanaAnteriorUnspecified = DateTime.SpecifyKind(finSemanaAnterior, DateTimeKind.Unspecified);

        var accesosSemanaActual = await context.AccesosDiaSemana
            .Where(a => a.Fecha >= inicioSemanaActualUnspecified && a.Fecha <= finSemanaActualUnspecified)
            .SumAsync(a => a.Cantidad, cancellationToken);

        var accesosSemanaAnterior = await context.AccesosDiaSemana
            .Where(a => a.Fecha >= inicioSemanaAnteriorUnspecified && a.Fecha <= finSemanaAnteriorUnspecified)
            .SumAsync(a => a.Cantidad, cancellationToken);

        var variacion = CalcularVariacionPorcentual(accesosSemanaActual, accesosSemanaAnterior);

        return new AccesosResponse(accesosSemanaActual, variacion);
    }

    private async Task<IncidenciasResponse> ObtenerTotalIncidencias(CancellationToken cancellationToken)
    {
        var (inicioSemanaActual, finSemanaActual) = DateTimeExtensions.GetCurrentWeekRange();
        var (inicioSemanaAnterior, finSemanaAnterior) = DateTimeExtensions.GetPreviousWeekRange();

        var inicioSemanaActualUnspecified = DateTime.SpecifyKind(inicioSemanaActual, DateTimeKind.Unspecified);
        var finSemanaActualUnspecified = DateTime.SpecifyKind(finSemanaActual, DateTimeKind.Unspecified);
        var inicioSemanaAnteriorUnspecified = DateTime.SpecifyKind(inicioSemanaAnterior, DateTimeKind.Unspecified);
        var finSemanaAnteriorUnspecified = DateTime.SpecifyKind(finSemanaAnterior, DateTimeKind.Unspecified);

        var incidenciasSemanaActual = await context.AccesosIncidencias
            .Where(i => i.Fecha >= inicioSemanaActualUnspecified && i.Fecha <= finSemanaActualUnspecified)
            .SumAsync(i => i.Cantidad, cancellationToken);

        var incidenciasSemanaAnterior = await context.AccesosIncidencias
            .Where(i => i.Fecha >= inicioSemanaAnteriorUnspecified && i.Fecha <= finSemanaAnteriorUnspecified)
            .SumAsync(i => i.Cantidad, cancellationToken);

        var variacion = CalcularVariacionPorcentual(incidenciasSemanaActual, incidenciasSemanaAnterior);

        return new IncidenciasResponse(incidenciasSemanaActual, variacion);
    }

    private async Task<HoraPicoResponse> ObtenerHoraPico(CancellationToken cancellationToken)
    {
        var fechaActual = DateTime.Today;

        var horaPico = await context.AccesosPorHora
            .Where(a => a.Fecha == fechaActual)
            .OrderByDescending(a => a.Cantidad)
            .Select(a => new { a.Hora, a.Cantidad })
            .FirstOrDefaultAsync(cancellationToken);

        if (horaPico == null)
            return new HoraPicoResponse("00:00", 0);

        var horaFormateada = horaPico.Hora.ToString("00") + ":00";

        return new HoraPicoResponse(horaFormateada, horaPico.Cantidad);
    }

    private async Task<int> ObtenerPromedioDiario(CancellationToken cancellationToken)
    {
        var fechaActual = DateTime.Today;

        var promedio = await context.AccesosPorHora
            .Where(a => a.Fecha == fechaActual)
            .Select(a => (double?)a.Cantidad)
            .DefaultIfEmpty()
            .AverageAsync(cancellationToken);

        return (int)Math.Round(promedio ?? 0);
    }

    private string CalcularVariacionPorcentual(int actual, int anterior)
    {
        if (anterior == 0) return "+0% vs sem. anterior";

        var diferencia = actual - anterior;
        var porcentaje = (diferencia * 100.0) / anterior;

        var simbolo = porcentaje >= 0 ? "+" : "-";
        return $"{simbolo} {Math.Abs((int)Math.Round(porcentaje))}% vs sem. anterior";
    }
}

