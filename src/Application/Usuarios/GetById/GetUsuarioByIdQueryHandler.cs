using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.GetById;

internal sealed class GetUsuarioByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetUsuarioByIdQuery, UsuarioByIdResponse>
{
    public async Task<Result<UsuarioByIdResponse>> Handle(
        GetUsuarioByIdQuery query,
        CancellationToken cancellationToken)
    {
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
            return Result.Failure<UsuarioByIdResponse>(UsuarioErrors.NotFound(query.UsuarioId));
        }

        var usuario = usuarioConRoles.Usuario;
        var roles = usuarioConRoles.Roles;

        if (!roles.Any())
        {
            return Result.Failure<UsuarioByIdResponse>(UsuarioErrors.WithoutRoles);
        }

        var estadoUsuario = new ItemResponse<int>((int)usuario.Estado, usuario.Estado.ToString());

        return new UsuarioByIdResponse(
            Email: usuario.Email,
            Nombre: usuario.Nombre,
            Apellido: usuario.Apellido,
            Roles: roles.Select(r => new ItemResponse<Guid>(
                r.Id,
                r.NombreRol)).ToList(),
            NumeroDocumento: usuario.NumeroDocumento,
            HorarioInicio: usuario.HorarioInicio,
            HorarioFin: usuario.HorarioFin,
            Estado: estadoUsuario);
    }
}