using Application.Abstractions.Messaging;

namespace Application.Dispositivos.Delete;

public sealed record DeleteDispositivoCommand(Guid DispositivoId) : ICommand;