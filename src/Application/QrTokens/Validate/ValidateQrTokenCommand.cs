using Application.Abstractions.Messaging;

namespace Application.QrTokens.Validate;

public sealed record ValidateQrTokenCommand(
    string QrToken,
    string DispositivoId) : ICommand<ValidateQrTokenResponse>;