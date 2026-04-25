using SharedKernel;

namespace Domain.Analytics.AccesosIncidencias;

public static class AccesoIncidenciaErrors
{
    public static Error NotFound(Guid accesoId) => Error.NotFound(
        "AccesoIncidencia.NotFound",
        $"No se encontró el registro de incidencia con Id = '{accesoId}'");

    public static readonly Error DuplicateEntry = Error.Conflict(
        "AccesoIncidencia.DuplicateEntry",
        "Ya existe un registro para este punto de control, incidencia y fecha");
}