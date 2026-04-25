using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Authentication.RefreshTokens;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IConfiguration configuration, 
    IDateTimeProvider dateTimeProvider,
    IApplicationDbContext context) : ITokenProvider
{
    public async Task<string> CreateAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        var parametroAutenticacion = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "autenticacion", cancellationToken);

        var permisos = await context.Usuarios
            .Where(u => u.Id == usuario.Id)
            .Join(context.UsuariosRoles,
                u => u.Id,
                ur => ur.UsuarioId,
                (u, ur) => ur)
            .Join(context.RolesPermisos,
                ur => ur.RolId,
                rp => rp.RolId,
                (ur, rp) => rp)
            .Join(context.Permisos,
                rp => rp.PermisoId,
                p => p.Id,
                (rp, p) => p.Nombre)
            .Distinct()
            .ToListAsync(cancellationToken);

        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim("nombre", usuario.Nombre),
            new Claim("apellido", usuario.Apellido)
        };

        foreach (var permiso in permisos)
        {
            claims.Add(new Claim("permission", permiso));
        }

        var jwtExpirationInMinutes = configuration.GetValue<int>("Jwt:ExpirationInMinutes");

        if (parametroAutenticacion is not null)
        {
            using var doc = JsonDocument.Parse(parametroAutenticacion.Valor);

            var vigencia = doc.RootElement
                .GetProperty("TiempoSesionMinutos")
                .GetInt32();

            jwtExpirationInMinutes = vigencia;
        }


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public RefreshToken CreateRefreshToken(Guid usuarioId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            FechaCreacion = dateTimeProvider.UtcNow,
            FechaExpiracion = dateTimeProvider.UtcNow.AddDays(30)
        };
    }
}
