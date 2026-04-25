using SharedKernel;

namespace Domain.QrTokens;

public static class QrTokenErrors
{
    public static Error NotFound(string token) => Error.NotFound(
        "QrTokens.NotFound",
        $"No se encontró el token QR = '{token}'");

    public static readonly Error AlreadyUsed = Error.Validation(
        "QrTokens.AlreadyUsed",
        "El token QR ya fue utilizado");

    public static readonly Error Expired = Error.Validation(
        "QrTokens.Expired",
        "El token QR ha expirado");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "QrTokens.UserNotFound",
        $"No se encontró el usuario con Id = '{userId}'");

    public static readonly Error InvalidTokenFormat = Error.Validation(
        "QrTokens.InvalidTokenFormat",
        "Formato de token inválido");

    public static readonly Error ZonePermissionDenied = Error.Validation(
        "QrTokens.ZonePermissionDenied",
        "El usuario no tiene permisos para acceder a esta zona");

    public static Error DispositivoNotFound(string dispositivoId) => Error.NotFound(
        "QrTokens.DispositivoNotFound",
        $"No se encontró el dispositivo con ID = '{dispositivoId}'");

    public static Error RolNotFound(Guid rolId) => Error.NotFound(
        "QrTokens.RolNotFound",
        $"No se encontró el rol con Id = '{rolId}'");

    public static Error UsuarioRolNotFound(Guid userId,Guid rolId) => Error.NotFound(
        "QrTokens.UsuarioRolNotFound",
        $"No se encontró la relacion de el rol con Id = '{rolId}' para el usuario con Id = '{userId}'");

    public static readonly Error NoZonaAvailableWithCapacity = Error.Validation(
        "QrTokens.NoZonaAvailableWithCapacity",
        "No hay zonas disponibles con capacidad para registrar una entrada.");

    public static readonly Error NoZonaAvailableWithSpaceUsed = Error.Validation(
        "QrTokens.NoZonaAvailableWithSpaceUsed",
        "No hay zonas con espacio utilizado para registrar una salida.");

    public static Error SeguridadQRNotFound() => Error.NotFound(
        "QrTokens.SeguridadQRNotFound",
        $"No se encontró la configuracion del parametro de seguridad_qr en la base de datos");
}