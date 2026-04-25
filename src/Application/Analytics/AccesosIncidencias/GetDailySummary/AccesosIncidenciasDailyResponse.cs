namespace Application.Analytics.AccesosIncidencias.GetDailySummary;

public record IncidenciaResponse(
    string Descripcion,
    int Cantidad);

public record AccesoIncidenciaDailyResponse(
    string NombrePuntoControl,
    DateTime Fecha,
    List<IncidenciaResponse> Incidencias);