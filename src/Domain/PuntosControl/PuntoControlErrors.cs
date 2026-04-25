using SharedKernel;

namespace Domain.PuntosControl;

public static class PuntoControlErrors
{
    public static Error NotFound(Guid puntoId) => Error.NotFound(
        "PuntosControl.NotFound",
        $"No se encontró el punto de control con Id = '{puntoId}'");

    public static readonly Error InvalidTipo = Error.Validation(
        "PuntosControl.InvalidTipo",
        "El tipo debe ser 'entrada' o 'salida'");

    public static readonly Error InvalidEstado = Error.Validation(
        "PuntosControl.InvalidEstado",
        "El estado debe ser 'activo' o 'inactivo'");

    public static readonly Error NombreNotUnique = Error.Conflict(
        "PuntosControl.NombreNotUnique",
        "Ya existe un punto de control con este nombre");

    public static Error CannotDeleteActive(Guid puntoId) => Error.Validation(
        "PuntosControl.CannotDeleteActive",
        $"No se puede eliminar el punto de control con Id = '{puntoId}' porque está activo");

    public static readonly Error NombreRequired = Error.Validation(
        "PuntosControl.NombreRequired",
        "El nombre del punto de control es requerido");

    public static readonly Error UbicacionRequired = Error.Validation(
        "PuntosControl.UbicacionRequired",
        "La ubicación del punto de control es requerida");
}