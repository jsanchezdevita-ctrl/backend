using Application.Abstractions.Messaging;

namespace Application.Dispositivos.Create;

public sealed record CreateDispositivoCommand(
    string DispositivoId,
    string Nombre,
    string DireccionIp,
    Guid PuntoControlId) : ICommand<Guid>;