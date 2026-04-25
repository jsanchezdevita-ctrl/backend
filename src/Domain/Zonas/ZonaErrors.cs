using SharedKernel;

namespace Domain.Zonas;

public static class ZonaErrors
{
    public static Error NotFound(Guid zonaId) => Error.NotFound(
        "Zonas.NotFound",
        $"No se encontró la zona con Id = '{zonaId}'");

    public static Error NotFoundByName(string nombre) => Error.NotFound(
        "Zonas.NotFoundByName",
        $"No se encontró una zona con el nombre '{nombre}'");

    public static readonly Error NameNotUnique = Error.Conflict(
        "Zonas.NameNotUnique",
        "El nombre de la zona proporcionado no es único");

    public static Error CannotDeleteWithRegistros(Guid zonaId) => Error.Conflict(
        "Zonas.CannotDeleteWithRegistros",
        $"No se puede eliminar la zona con Id = '{zonaId}' porque está referenciada en registros históricos de ingresos/egresos.");

    public static Error CannotDeleteWithEspacioUtilizado(Guid zonaId) => Error.Conflict(
        "Zonas.CannotDeleteWithEspacioUtilizado",
        $"No se puede eliminar la zona con Id = '{zonaId}' porque tiene roles con espacio utilizado. Libere el espacio primero.");
}