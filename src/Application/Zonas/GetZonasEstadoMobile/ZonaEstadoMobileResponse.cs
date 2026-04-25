namespace Application.Zonas.GetZonasEstadoMobile;

public sealed record ZonaEstadoMobileResponse(
    Guid ZonaId,
    string Nombre,
    int CapacidadMaxima,
    int EspacioUtilizado);