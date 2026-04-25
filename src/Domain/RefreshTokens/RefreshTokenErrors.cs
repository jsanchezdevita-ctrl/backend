using SharedKernel;

namespace Domain.Authentication.RefreshTokens;

public static class RefreshTokenErrors
{
    public static readonly Error Invalid = Error.Failure(
        "RefreshToken.Invalid",
        "El refresh token no es válido.");

    public static readonly Error Expired = Error.Failure(
        "RefreshToken.Expired",
        "El refresh token expiró.");

    public static readonly Error Revoked = Error.Failure(
        "RefreshToken.Revoked",
        "El refresh token fue revocado.");

    public static Error NotFound(Guid tokenId) => Error.NotFound(
        "RefreshToken.NotFound",
        $"No se encontró el refresh token con Id = '{tokenId}'");
}
