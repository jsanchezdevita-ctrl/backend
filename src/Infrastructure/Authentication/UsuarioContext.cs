using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal sealed class UsuarioContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UsuarioId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUsuarioId() ??
        throw new ApplicationException("El contexto del usuario no está disponible");

    public string Email =>
    _httpContextAccessor
        .HttpContext?
        .User
        .GetEmail() ?? 
    throw new ApplicationException("El email del usuario no está disponible");

}
