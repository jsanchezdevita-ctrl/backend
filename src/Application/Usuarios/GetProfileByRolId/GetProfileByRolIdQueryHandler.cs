using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.GetProfileByRolId;

internal sealed class GetUsuarioProfileQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetProfileByRolIdQuery, UsuarioProfileByRolResponse>
{
    public async Task<Result<UsuarioProfileByRolResponse>> Handle(
        GetProfileByRolIdQuery query,
        CancellationToken cancellationToken)
    {
        if (query.UsuarioId != userContext.UsuarioId)
        {
            return Result.Failure<UsuarioProfileByRolResponse>(UsuarioErrors.Unauthorized());
        }

        var usuarioConRol = await context.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == query.UsuarioId && !u.Deleted)
            .Select(u => new
            {
                Usuario = u,
                Rol = context.UsuariosRoles
                    .Where(ur => ur.UsuarioId == u.Id && !ur.Deleted && ur.RolId == query.RolId)
                    .Join(context.Roles.Where(r => !r.Deleted),
                        ur => ur.RolId,
                        r => r.Id,
                        (ur, r) => r)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (usuarioConRol?.Usuario is null)
        {
            return Result.Failure<UsuarioProfileByRolResponse>(UsuarioErrors.NotFound(query.UsuarioId));
        }

        if (usuarioConRol?.Rol is null)
        {
            return Result.Failure<UsuarioProfileByRolResponse>(UsuarioErrors.WithoutRoles);
        }

        var usuario = usuarioConRol.Usuario;
        var rol = usuarioConRol.Rol;

        List<string> zonas;

        zonas = await context.ZonasRoles
            .AsNoTracking()
            .Where(zr => zr.RolId == query.RolId && !zr.Deleted)
            .Join(context.Zonas.Where(z => !z.Deleted),
                zr => zr.ZonaId,
                z => z.Id,
                (zr, z) => z.Nombre)
            .Distinct()
            .ToListAsync(cancellationToken);

        var nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        bool esAdmin = rol.EsAdmin;

        return new UsuarioProfileByRolResponse(
            UsuarioId: usuario.Id,
            Email: usuario.Email,
            NombreCompleto: nombreCompleto,
            Rol: rol.NombreRol,
            EsAdmin: esAdmin,
            NumeroDocumento: usuario.NumeroDocumento,
            HorarioInicio: usuario.HorarioInicio,
            HorarioFin: usuario.HorarioFin,
            Estado: usuario.Estado.ToString(),
            FechaRegistro: usuario.FechaRegistro,
            Zonas: zonas);
    }
}