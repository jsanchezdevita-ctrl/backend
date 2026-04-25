using SharedKernel;

namespace Domain.ZonasPuntosControl;

public static class ZonaPuntoControlErrors
{
    public static Error NotFound(Guid zonaPuntoControlId) => Error.NotFound(
        "ZonasPuntosControl.NotFound",
        $"No se encontró la relación zona-punto control con Id = '{zonaPuntoControlId}'");

    public static readonly Error DuplicatePuntoControlForZona = Error.Conflict(
        "ZonasPuntosControl.DuplicatePuntoControlForZona",
        "Ya existe este punto de control asignado a la zona");

    public static Error NotFoundByZonaAndPuntoControl(Guid zonaId, Guid puntoControlId) => Error.NotFound(
        "ZonasPuntosControl.NotFoundByZonaAndPuntoControl",
        $"No se encontró la relación para ZonaId='{zonaId}' y PuntoControlId='{puntoControlId}'");
}