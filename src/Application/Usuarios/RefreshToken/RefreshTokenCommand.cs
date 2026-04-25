using Application.Abstractions.Messaging;
using Application.Usuarios.Login;

namespace Application.Usuarios.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<LoginResponse>;
