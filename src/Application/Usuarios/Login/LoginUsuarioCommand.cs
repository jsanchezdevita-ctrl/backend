using Application.Abstractions.Messaging;

namespace Application.Usuarios.Login;

public sealed record LoginUsuarioCommand(string Email, string Password) : ICommand<LoginResponse>;
