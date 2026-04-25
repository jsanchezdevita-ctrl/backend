using SharedKernel;

namespace Domain.ZonasRoles;

public static class ZonaRolErrors
{
    public static Error NotFound(Guid zonaRolId) => Error.NotFound(
        "ZonasRoles.NotFound",
        $"No se encontró la relación zona-rol con Id = '{zonaRolId}'");

    public static readonly Error DuplicateRolForZona = Error.Conflict(
        "ZonasRoles.DuplicateRolForZona",
        "Ya existe este rol asignado a la zona");

    public static Error CapacidadExcedida(int capacidad, int utilizado) => Error.Validation(
        "ZonasRoles.CapacidadExcedida",
        $"La capacidad máxima ({capacidad}) ha sido excedida. Espacio utilizado: {utilizado}");

    public static Error EspacioInsuficiente(int requerido, int disponible) => Error.Validation(
        "ZonasRoles.EspacioInsuficiente",
        $"Espacio insuficiente. Se requieren {requerido} unidades, solo hay {disponible} disponibles");

    public static Error NotFoundByZonaAndRol(Guid zonaId, Guid rolId) => Error.NotFound(
        "ZonasRoles.NotFoundByZonaAndRol",
        $"No se encontró la relación para ZonaId='{zonaId}' y RolId='{rolId}'");

    public static readonly Error CapacidadNoValida = Error.Validation(
    "ZonasRoles.CapacidadNoValida",
    "La capacidad máxima debe ser mayor a cero");

    public static Error CapacidadMenorAUtilizado(int capacidad, int utilizado) => Error.Validation(
        "ZonasRoles.CapacidadMenorAUtilizado",
        $"La nueva capacidad ({capacidad}) no puede ser menor al espacio utilizado ({utilizado})");

    public static Error CannotDeleteWithEspacioUtilizado(Guid zonaRolId, int espacioUtilizado) => Error.Conflict(
        "ZonasRoles.CannotDeleteWithEspacioUtilizado",
        $"No se puede eliminar la relación zona-rol con Id = '{zonaRolId}' porque tiene {espacioUtilizado} unidades de espacio utilizado");
}