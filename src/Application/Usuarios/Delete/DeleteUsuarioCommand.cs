using Application.Abstractions.Messaging;

namespace Application.Usuarios.Delete;

public sealed record DeleteUsuarioCommand(Guid UsuarioId) : ICommand;