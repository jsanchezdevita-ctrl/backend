using SharedKernel;

namespace Domain.Analytics.AccesosDiaSemana;

public static class AccesoDiaSemanaErrors
{
    public static Error NotFound(Guid accesoId) => Error.NotFound(
        "AccesoDiaSemana.NotFound",
        $"No se encontró el registro de acceso por día de semana con Id = '{accesoId}'");

    public static readonly Error DuplicateEntry = Error.Conflict(
        "AccesoDiaSemana.DuplicateEntry",
        "Ya existe un registro para esta fecha y día de semana");
}