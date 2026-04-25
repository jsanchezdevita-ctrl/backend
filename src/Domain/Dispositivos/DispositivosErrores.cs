using SharedKernel;

namespace Domain.Dispositivos;

public static class DispositivosErrores
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Dispositivos.NotFound",
        $"No se encontró el dispositivo con Id = '{id}'");

    public static readonly Error DispositivoIdNoUnico = Error.Conflict(
        "Dispositivos.DispositivoIdNoUnico",
        "El ID del dispositivo ya está registrado");

    public static readonly Error DireccionIpNoUnica = Error.Conflict(
        "Dispositivos.DireccionIpNoUnica",
        "La dirección IP ya está en uso por otro dispositivo");

    public static readonly Error NombreNoUnico = Error.Conflict(
        "Dispositivos.NombreNoUnico",
        "El nombre del dispositivo ya está registrado");

    public static readonly Error DispositivoIdRequerido = Error.Validation(
        "Dispositivos.DispositivoIdRequerido",
        "El ID del dispositivo es requerido");

    public static readonly Error NombreRequerido = Error.Validation(
        "Dispositivos.NombreRequerido",
        "El nombre del dispositivo es requerido");

    public static readonly Error DireccionIpRequerida = Error.Validation(
        "Dispositivos.DireccionIpRequerida",
        "La dirección IP es requerida");

    public static readonly Error DireccionIpInvalida = Error.Validation(
        "Dispositivos.DireccionIpInvalida",
        "La dirección IP tiene un formato inválido");

    public static Error ConectadoCannotDeleted(Guid id) => Error.Validation(
        "Dispositivos.ConectadoCannotDeleted",
        $"No se puede eliminar el dispositivo con Id = '{id}' porque está conectado");

    public static readonly Error PuntoControlYaEnUso = Error.Conflict(
        "Dispositivos.PuntoControlYaEnUso",
        "El punto de control ya está asignado a otro dispositivo");
}