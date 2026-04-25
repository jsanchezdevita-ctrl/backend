namespace Application.Parametros.GetMobile;

public sealed record MobileConfigResponse(
    int IntervaloRenovacionMinutos,
    bool BloqueoAutomaticoPuertas,
    int TiempoBloqueoSegundos);