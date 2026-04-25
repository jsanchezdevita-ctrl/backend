namespace Application.Analytics.AccesosTipoUsuario.GetWeeklySummary;

public record AccesosTipoUsuarioSummaryResponse(
    string TipoUsuario,
    int Total);