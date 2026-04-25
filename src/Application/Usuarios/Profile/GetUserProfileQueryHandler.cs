using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.Profile;

public class GetUserProfileQueryHandler(
    IUserContext userContext,
    IApplicationDbContext context
    ) : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    public async Task<Result<UserProfileResponse>> Handle(
        GetUserProfileQuery query, 
        CancellationToken cancellationToken)
    {
        var usuarioConRol = await context.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == userContext.UsuarioId && !u.Deleted)
            .Select(u => new
            {
                Usuario = u,
                Rol = context.UsuariosRoles
                    .Where(ur => ur.UsuarioId == u.Id && !ur.Deleted)
                    .Join(context.Roles.Where(r => !r.Deleted),
                        ur => ur.RolId,
                        r => r.Id,
                        (ur, r) => r)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (usuarioConRol?.Usuario is null)
        {
            return Result.Failure<UserProfileResponse>(UsuarioErrors.NotFoundByEmail);
        }

        var profile = new UserProfileResponse(
            usuarioConRol.Usuario.Id,
            $"{usuarioConRol.Usuario.Nombre} {usuarioConRol.Usuario.Apellido}",
            usuarioConRol.Usuario.Email,
            usuarioConRol.Rol?.NombreRol ?? "Sin Rol",
            usuarioConRol.Rol?.EsAdmin ?? false
        );

        return profile;
    }
}
