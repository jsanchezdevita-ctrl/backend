namespace Application.Zonas.GetZonasEstadoWeb;

public sealed record ZonaEstadoWebResponse(
    Guid ZonaId,
    string Nombre,
    int CapacidadMaxima,
    int EspacioUtilizado);