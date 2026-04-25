using Domain.Authentication.RefreshTokens;
using Domain.Usuarios;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    Task<string> CreateAsync(Usuario usuario, CancellationToken cancellationToken = default);
    RefreshToken CreateRefreshToken(Guid usuarioId);
}
