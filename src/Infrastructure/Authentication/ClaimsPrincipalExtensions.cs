using System.Security.Claims;

namespace Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUsuarioId(this ClaimsPrincipal? principal)
    {
        string? usuarioId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(usuarioId, out Guid parsedUsuarioId) ?
            parsedUsuarioId :
            throw new ApplicationException("El ID de usuario no está disponible");
    }

    public static string GetEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.Email) ??
               throw new ApplicationException("El email del usuario no está disponible");
    }

}
