using SharedKernel;

namespace Domain.Parametros;

public static class ParametrosHistorialErrores
{
    public static Error NotFound(Guid historialId) => Error.NotFound(
        "ParametrosHistorial.NotFound",
        $"No se encontró el historial con Id = '{historialId}'");

    public static Error VersionNotFound(string llave, int version) => Error.NotFound(
        "ParametrosHistorial.VersionNotFound",
        $"No se encontró la versión {version} para la llave '{llave}'");

    public static Error NoHistoryFound(string llave) => Error.NotFound(
        "ParametrosHistorial.NoHistoryFound",
        $"No se encontró historial para la llave '{llave}'");
}