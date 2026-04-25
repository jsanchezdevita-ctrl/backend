using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Usuarios.Login;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider)
    : ICommandHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == command.RefreshToken, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked ||
            refreshToken.FechaExpiracion < DateTime.UtcNow)
        {
            return Result.Failure<LoginResponse>(UsuarioErrors.Unauthorized());
        }

        var usuarioConRoles = await context.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == refreshToken.UsuarioId && !u.Deleted)
            .Select(u => new
            {
                Usuario = u,
                Roles = context.UsuariosRoles
                    .Where(ur => ur.UsuarioId == u.Id && !ur.Deleted)
                    .Join(context.Roles.Where(r => !r.Deleted),
                        ur => ur.RolId,
                        r => r.Id,
                        (ur, r) => r)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (usuarioConRoles?.Usuario is null)
        {
            return Result.Failure<LoginResponse>(UsuarioErrors.NotFound(refreshToken.UsuarioId));
        }

        var usuario = usuarioConRoles.Usuario;
        var roles = usuarioConRoles.Roles;

        if (!roles.Any())
        {
            return Result.Failure<LoginResponse>(UsuarioErrors.WithoutRoles);
        }

        string newToken = await tokenProvider.CreateAsync(usuario);
        var newRefreshToken = tokenProvider.CreateRefreshToken(usuario.Id);

        refreshToken.Revoke();

        await context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);


        bool esAdmin = roles.Any(r => r.EsAdmin);

        return new LoginResponse(
            AccessToken: newToken,
            RefreshToken: newRefreshToken.Token,
            UsuarioId: usuario.Id,
            Email: usuario.Email,
            NombreCompleto: $"{usuario.Nombre} {usuario.Apellido}",
            Roles: roles.Select(r => r.NombreRol).ToList(),
            RolesInfo: roles.Select(r => new ItemResponse<Guid>
            (
                r.Id,
                r.NombreRol
            )).ToList(),
            EsAdmin: esAdmin,
            NumeroDocumento: usuario.NumeroDocumento);
    }
}