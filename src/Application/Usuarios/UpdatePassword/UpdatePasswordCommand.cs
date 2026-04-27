using Application.Abstractions.Messaging;

namespace Application.Usuarios.UpdatePassword;

public sealed record UpdatePasswordCommand(
    Guid UsuarioId,
    string NewPassword) : ICommand;
