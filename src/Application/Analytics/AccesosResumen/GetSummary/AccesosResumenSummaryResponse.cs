namespace Application.Analytics.AccesosResumen.GetSummary;

public record AccesosResumenSummaryResponse(
    AccesosResponse TotalAccesos, 
    IncidenciasResponse TotalIncidencias,
    HoraPicoResponse HoraPico,
    int PromedioDiario);

public record AccesosResponse(
    int Cantidad,
    string VariacionSemanaAnterior);

public record IncidenciasResponse(
    int Cantidad,
    string VariacionSemanaAnterior);

public record HoraPicoResponse(
    string Hora,
    int Cantidad);