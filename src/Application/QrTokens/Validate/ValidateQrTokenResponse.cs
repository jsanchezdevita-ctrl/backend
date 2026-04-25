
namespace Application.QrTokens.Validate;

public sealed record ValidateQrTokenResponse(
    Guid Id,
    bool isSuccess);