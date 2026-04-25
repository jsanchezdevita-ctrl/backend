using Application.Abstractions.Messaging;

namespace Application.Dispositivos.Update;

public sealed record UpdateDispositivoCommand(
    Guid DispositivoId,
    string DispositivoIdCodigo,
    string Nombre,
    string DireccionIp,
    Guid PuntoControlId) : ICommand;