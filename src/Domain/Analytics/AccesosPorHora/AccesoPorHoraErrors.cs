using SharedKernel;

namespace Domain.Analytics.AccesosPorHora;

public static class AccesoPorHoraErrors
{
    public static Error NotFound(Guid accesoId) => Error.NotFound(
        "AccesoPorHora.NotFound",
        $"No se encontró el registro de acceso por hora con Id = '{accesoId}'");

    public static readonly Error DuplicateEntry = Error.Conflict(
        "AccesoPorHora.DuplicateEntry",
        "Ya existe un registro para esta fecha y hora");
}