using Application.Abstractions.Messaging;

namespace Application.Usuarios.UpdateFcmToken;

public sealed record UpdateUsuarioFcmTokenCommand(string FcmToken) : ICommand;