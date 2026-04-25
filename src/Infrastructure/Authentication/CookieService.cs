using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public class CookieService : ICookieService
{
    private const string AccessTokenCookieName = "access_token";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CookieConfigOptions _cookieConfigOptions;

    public CookieService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<CookieConfigOptions> cookieOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _cookieConfigOptions = cookieOptions.Value;
    }

    public void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = _cookieConfigOptions.HttpOnly,
            Secure = _cookieConfigOptions.Secure,
            SameSite = _cookieConfigOptions.SameSiteMode,
            Expires = DateTime.UtcNow.AddDays(7) // O calcula del token
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            AccessTokenCookieName,
            token,
            cookieOptions);
    }

    public void DeleteTokenCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(AccessTokenCookieName);
    }
}