namespace Application.QrTokens.Generate;

public sealed record QrTokenResponse(
    string Token,
    DateTime FechaExpiracion);