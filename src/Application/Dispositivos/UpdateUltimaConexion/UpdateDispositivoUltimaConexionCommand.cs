using Application.Abstractions.Messaging;

namespace Application.Dispositivos.UpdateUltimaConexion;

public sealed record UpdateDispositivoUltimaConexionCommand(
    Guid DispositivoId) : ICommand;