using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.GetForManagement;

internal sealed class GetUsuariosForManagementQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetUsuariosForManagementQuery, UsuariosForManagementResponse>
{
    public async Task<Result<UsuariosForManagementResponse>> Handle(
        GetUsuariosForManagementQuery query,
        CancellationToken cancellationToken)
    {
        var usuariosQuery = context.Usuarios.OrderByDescending(o => o.FechaRegistro).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            usuariosQuery = usuariosQuery.SearchByTerm(query.SearchTerm,
                u => u.NumeroDocumento,
                u => u.Email,
                u => u.Nombre,
                u => u.Apellido);
        }

        var usuariosPaginados = await usuariosQuery
            .Select(u => new { u.Id })
            .Distinct()
            .ToPagedResponseAsync(query.Page, query.PageSize, cancellationToken);

        var usuarioIds = usuariosPaginados.Items.Select(x => x.Id).ToList();

        var usuariosCompletos = await context.Usuarios
            .Where(u => usuarioIds.Contains(u.Id))
            .Select(u => new
            {
                u.Id,
                u.NumeroDocumento,
                u.Email,
                u.Nombre,
                u.Apellido,
                u.Estado,
                u.HorarioInicio,
                u.HorarioFin,
                u.FechaRegistro,
                Roles = context.UsuariosRoles
                    .Where(ur => ur.UsuarioId == u.Id)
                    .Join(context.Roles,
                        ur => ur.RolId,
                        r => r.Id,
                        (ur, r) => new RolResponse(r.Id, r.NombreRol, r.Descripcion, r.EsAdmin))
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        var items = usuariosCompletos.Select(u => new UsuarioConRolResponse(
            u.Id,
            u.NumeroDocumento,
            u.Email,
            u.Nombre,
            u.Apellido,
            u.Estado.ToString(),
            u.HorarioInicio,
            u.HorarioFin,
            u.FechaRegistro,
            u.Roles)).ToList();

        var usuariosResponse = new PagedResponse<UsuarioConRolResponse>(
            items,
            usuariosPaginados.Page,
            usuariosPaginados.PageSize,
            usuariosPaginados.TotalCount,
            usuariosPaginados.TotalPages);

        var roles = await context.Roles
            .Select(r => new RolMetadata(r.Id, r.NombreRol, r.Descripcion))
            .ToListAsync(cancellationToken);

        var estados = Enum.GetValues(typeof(UsuarioState))
            .Cast<UsuarioState>()
            .Select(e => new StatuMetadata(
                (int)e,
                e.ToString(),
                GetDescriptionStatus(e)))
            .ToList();

        return new UsuariosForManagementResponse(usuariosResponse, roles, estados);
    }

    private static string GetDescriptionStatus(UsuarioState estado)
    {
        return estado switch
        {
            UsuarioState.Habilitado => "Usuario activo en el sistema",
            UsuarioState.Restringido => "Usuario con acceso restringido",
            UsuarioState.EnRevision => "Usuario pendiente de aprobación",
            _ => "Estado desconocido"
        };
    }
}