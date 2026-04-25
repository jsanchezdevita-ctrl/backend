using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.GetMetadata;

internal sealed class GetUsuariosMetadataQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetUsuariosMetadataQuery, UsuariosMetadataResponse>
{
    public async Task<Result<UsuariosMetadataResponse>> Handle(
        GetUsuariosMetadataQuery query,
        CancellationToken cancellationToken)
    {

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

        return new UsuariosMetadataResponse(roles, estados);
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