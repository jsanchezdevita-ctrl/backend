namespace Application.Analytics.AccesosDiaSemana.GetWeeklySummary;

public record AccesosDiaSemanaSummaryResponse(
    string DiaSemanaCompleto,
    string DiaSemanaCorto,
    int Total);