using SharedKernel;

namespace Domain.Permisos;

public static class PermisoErrors
{
    public static Error NotFound(Guid permisoId) => Error.NotFound(
        "Permisos.NotFound",
        $"No se encontró el permiso con Id = '{permisoId}'");

    public static readonly Error NameNotUnique = Error.Conflict(
        "Permisos.NameNotUnique",
        "El nombre del permiso ya existe.");
}
