using SharedKernel;

namespace Domain.Usuarios;

public static class UsuarioErrors
{
    public static Error NotFound(Guid usuarioId) => Error.NotFound(
        "Usuarios.NotFound",
        $"No se encontró el usuario con Id = '{usuarioId}'");

    public static Error Unauthorized() => Error.Failure(
        "Usuarios.Unauthorized",
        "No está autorizado para realizar esta acción.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Usuarios.NotFoundByEmail",
        "No se encontró un usuario con el email especificado");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Usuarios.EmailNotUnique",
        "El email proporcionado no es único");

    public static readonly Error CannotDeactivateSelf = Error.Validation(
        "Usuarios.CannotDeactivateSelf",
        "No puede desactivar su propio usuario");

    public static readonly Error CannotDeleteSelf = Error.Validation(
        "Usuarios.CannotDeleteSelf",
        "No puedes eliminar tu propia cuenta de usuario.");

    public static readonly Error CannotDeleteAdminUsers = Error.Validation(
        "Usuarios.CannotDeleteAdminUsers",
        "No se pueden eliminar usuarios con roles de administrador.");

    public static readonly Error HorarioInvalido = Error.Validation(
        "Usuarios.HorarioInvalido",
        "El horario de inicio y fin no pueden ser iguales");

    public static readonly Error NumeroDocumentoNotUnique = Error.Conflict(
        "Usuarios.NumeroDocumentoNotUnique",
        "El número de documento proporcionado no es válido porque está duplicado");

    public static readonly Error WithoutRoles = Error.Validation(
        "Usuarios.WithoutRoles",
        "El usuario no cuenta con roles asignados actualmente");

}
