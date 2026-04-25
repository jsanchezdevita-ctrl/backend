using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class CookieConfigOptions
{
    public bool HttpOnly { get; set; } = true;
    public bool Secure { get; set; } = true;
    public SameSiteMode SameSiteMode { get; set; } = SameSiteMode.Strict;
}