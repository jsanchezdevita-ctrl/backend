using SharedKernel;

namespace Domain.RegistrosIngresosEgresos;

public static class RegistroIngresoEgresoErrors
{
    public static Error NotFound(Guid registroId) => Error.NotFound(
        "RegistrosIngresosEgresos.NotFound",
        $"No se encontró el registro con Id = '{registroId}'");

    public static readonly Error InvalidResultado = Error.Failure(
        "RegistrosIngresosEgresos.InvalidResultado",
        "El resultado debe ser 'autorizado' o 'denegado'");

    public static readonly Error PuntoControlRequired = Error.Validation(
        "RegistrosIngresosEgresos.PuntoControlRequired",
        "Debe proporcionar al menos un punto de entrada o salida");

    public static Error ZonaRolNotFound(Guid zonaRolId) => Error.NotFound(
        "RegistrosIngresosEgresos.ZonaRolNotFound",
        $"No se encontró la relación zona-rol con Id = '{zonaRolId}'");

    public static Error PuntoControlNotFound(Guid puntoControlId, string tipo) => Error.NotFound(
        "RegistrosIngresosEgresos.PuntoControlNotFound",
        $"No se encontró el punto de {tipo} con Id = '{puntoControlId}'");

    public static Error UsuarioNotFound(Guid usuarioId) => Error.NotFound(
        "RegistrosIngresosEgresos.UsuarioNotFound",
        $"No se encontró el usuario con Id = '{usuarioId}'");

    public static Error EstadoRegistroNotFound(Guid estadoRegistroId) => Error.NotFound(
        "RegistrosIngresosEgresos.EstadoRegistroNotFound",
        $"No se encontró el estado de registro con Id = '{estadoRegistroId}'");

    public static readonly Error ConcurrencyError = Error.Conflict(
        "RegistrosIngresosEgresos.ConcurrencyError",
        "El registro ha sido modificado por otro usuario. Por favor, intente nuevamente.");

    public static Error CapacidadExcedida(int capacidad, int utilizado, string detalle) => Error.Validation(
        "RegistrosIngresosEgresos.CapacidadExcedida",
        $"Capacidad máxima excedida. Capacidad: {capacidad}, Utilizado: {utilizado}. {detalle}");

    public static Error EspacioInsuficiente(int utilizado, string detalle) => Error.Validation(
        "RegistrosIngresosEgresos.EspacioInsuficiente",
        $"Espacio insuficiente. Utilizado actual: {utilizado}. {detalle}");

    public static readonly Error AmbosPuntosNoPermitidos = Error.Validation(
        "RegistrosIngresosEgresos.AmbosPuntosNoPermitidos",
        "No se permiten puntos de entrada y salida simultáneamente");

    public static readonly Error InvalidSequenceEntryWithoutExit = Error.Validation(
    "Registros.InvalidSequence",
    "Ya tiene una entrada activa. Debe registrar una salida primero");

    public static readonly Error InvalidSequenceExitWithoutEntry = Error.Validation(
        "Registros.InvalidSequence",
        "No tiene una entrada activa para registrar una salida");

    public static readonly Error InvalidSequenceFirstRecordMustBeEntry = Error.Validation(
        "Registros.InvalidSequence",
        "El primer registro debe ser una entrada");
}
