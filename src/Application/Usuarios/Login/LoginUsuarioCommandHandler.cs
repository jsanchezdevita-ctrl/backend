using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.Login;

internal sealed class LoginUsuarioCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    ICookieService cookieService) : ICommandHandler<LoginUsuarioCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        var usuarioConRoles = await context.Usuarios
            .AsNoTracking()
            .Where(u => u.Email == command.Email && !u.Deleted)
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
            return Result.Failure<LoginResponse>(UsuarioErrors.NotFoundByEmail);
        }

        var usuario = usuarioConRoles.Usuario;
        var roles = usuarioConRoles.Roles;

        if (!roles.Any())
        {
            return Result.Failure<LoginResponse>(UsuarioErrors.WithoutRoles);
        }

        bool verified = passwordHasher.Verify(command.Password, usuario.PasswordHash);
        if (!verified)
        {
            return Result.Failure<LoginResponse>(UsuarioErrors.NotFoundByEmail);
        }

        string accessToken = await tokenProvider.CreateAsync(usuario, cancellationToken);
        var refreshToken = tokenProvider.CreateRefreshToken(usuario.Id);

        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        cookieService.SetTokenCookie(accessToken);

        bool esAdmin = roles.Any(r => r.EsAdmin);

        return new LoginResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token,
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