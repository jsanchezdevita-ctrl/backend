using SharedKernel;

namespace Domain.Notifications;

public static class FcmErrors
{
    public static readonly Error TokenNotProvided = Error.Problem(
        "Fcm.TokenNotProvided",
        "Token FCM no proporcionado");

    public static readonly Error ConfigurationMissing = Error.Problem(
        "Fcm.ConfigurationMissing",
        "Configuración de FCM incompleta");

    public static readonly Error AccessTokenFailed = Error.Problem(
        "Fcm.AccessTokenFailed",
        "No se pudo obtener el token de acceso para FCM");

    public static readonly Error SendNotificationFailed = Error.Problem(
        "Fcm.SendNotificationFailed",
        "Error al enviar notificación FCM");
}