namespace Application.Abstractions.Authentication;

public interface ICookieService
{
    void SetTokenCookie(string token);
    void DeleteTokenCookie();
}