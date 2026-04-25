using Application.Abstractions.Messaging;

namespace Application.RegistrosIngresosEgresos.Create;

public sealed record CreateRegistroIngresoEgresoCommand(
    Guid UsuarioId,
    Guid? PuntoEntradaId,
    Guid? PuntoSalidaId,
    Guid EstadoRegistroId,
    Guid? ZonaRolId,
    String? Observacion) : ICommand<Guid>;