using Application.Abstractions.Messaging;

namespace Application.QrTokens.Generate;

public sealed record GenerateQrTokenCommand(Guid UsuarioId, Guid RolId) : ICommand<QrTokenResponse>;