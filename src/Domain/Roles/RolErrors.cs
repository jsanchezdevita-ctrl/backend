using SharedKernel;

namespace Domain.Roles;

public static class RolErrors
{
    public static Error NotFound(Guid rolId) => Error.NotFound(
        "Roles.NotFound",
        $"No se encontró el rol con Id = '{rolId}'");

    public static readonly Error NotFoundByName = Error.NotFound(
        "Roles.NotFoundByName",
        "No se encontró un rol con el nombre especificado");

    public static readonly Error NameNotUnique = Error.Conflict(
        "Roles.NameNotUnique",
        "El nombre del rol proporcionado no es único");
}
