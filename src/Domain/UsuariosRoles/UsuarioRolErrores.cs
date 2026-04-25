using SharedKernel;

namespace Domain.UsuariosRoles;

public static class UsuarioRolErrors
{
    public static Error NotFound(Guid usuarioId, Guid rolId) => Error.NotFound(
        "UsuariosRoles.NotFound",
        $"No se encontró la asignación del rol con IdRol = '{rolId}' para el usuario Id = '{usuarioId}'");

    public static readonly Error AlreadyAssigned = Error.Conflict(
        "UsuariosRoles.AlreadyAssigned",
        "El rol ya está asignado a este usuario");

    public static readonly Error OneRole = Error.Validation(
    "UsuariosRoles.OneRole",
    "El usuario solo puede tener un rol asignado por vez");
}
