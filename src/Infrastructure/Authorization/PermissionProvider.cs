using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(IApplicationDbContext context)
{

    public async Task<HashSet<string>> GetForUsuarioIdAsync(Guid usuarioId)
    {
        var permisos = await context.Usuarios
            .Where(u => u.Id == usuarioId)
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
            .ToListAsync();

        HashSet<string> permissionsSet = permisos.ToHashSet();

        return permissionsSet;
    }
}
