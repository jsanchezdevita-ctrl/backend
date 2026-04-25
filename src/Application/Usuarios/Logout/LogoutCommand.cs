using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Usuarios.Logout;

public sealed record LogoutCommand(/*string RefreshToken*/) : ICommand<Unit>;
