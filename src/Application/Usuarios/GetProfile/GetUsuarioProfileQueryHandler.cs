using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.GetProfile;

internal sealed class GetUsuarioProfileQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetUsuarioProfileQuery, UsuarioProfileResponse>
{
    public async Task<Result<UsuarioProfileResponse>> Handle(
        GetUsuarioProfileQuery query,
        CancellationToken cancellationToken)
    {
        //TODO emite el Failure pero se salta por una excepcion 500 sin el mensaje (Verificar)
        if (query.UsuarioId != userContext.UsuarioId)
        {
            return Result.Failure<UsuarioProfileResponse>(UsuarioErrors.Unauthorized());
        }

        var usuarioConRoles = await context.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == query.UsuarioId && !u.Deleted)
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
            return Result.Failure<UsuarioProfileResponse>(UsuarioErrors.NotFound(query.UsuarioId));
        }

        var usuario = usuarioConRoles.Usuario;
        var roles = usuarioConRoles.Roles;

        if (!roles.Any())
        {
            return Result.Failure<UsuarioProfileResponse>(UsuarioErrors.WithoutRoles);
        }

        if (query.RolId.HasValue)
        {
            var rolEspecifico = roles.FirstOrDefault(r => r.Id == query.RolId.Value);
            if (rolEspecifico is null)
            {
                return Result.Failure<UsuarioProfileResponse>(UsuarioErrors.Unauthorized());
            }
        }

        List<string> zonas;
        if (query.RolId.HasValue)
        {
            zonas = await context.ZonasRoles
                .AsNoTracking()
                .Where(zr => zr.RolId == query.RolId.Value && !zr.Deleted)
                .Join(context.Zonas.Where(z => !z.Deleted),
                    zr => zr.ZonaId,
                    z => z.Id,
                    (zr, z) => z.Nombre)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
        else
        {
            var rolIds = roles.Select(r => r.Id).ToList();
            zonas = await context.ZonasRoles
                .AsNoTracking()
                .Where(zr => rolIds.Contains(zr.RolId) && !zr.Deleted)
                .Join(context.Zonas.Where(z => !z.Deleted),
                    zr => zr.ZonaId,
                    z => z.Id,
                    (zr, z) => z.Nombre)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        var nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        bool esAdmin = roles.Any(r => r.EsAdmin);

        return new UsuarioProfileResponse(
            UsuarioId: usuario.Id,
            Email: usuario.Email,
            NombreCompleto: nombreCompleto,
            Roles: roles.Select(r => new RolResponse(
                r.Id,
                r.NombreRol,
                r.Descripcion,
                r.EsAdmin)).ToList(),
            EsAdmin: esAdmin,
            NumeroDocumento: usuario.NumeroDocumento,
            HorarioInicio: usuario.HorarioInicio,
            HorarioFin: usuario.HorarioFin,
            Estado: usuario.Estado.ToString(),
            FechaRegistro: usuario.FechaRegistro,
            Zonas: zonas);
    }
}