using Application.Abstractions.Messaging;
using Application.Usuarios.GetProfileByRolId;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetProfileByRolId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/{usuarioId}/profile-rol", async (
            Guid usuarioId,
            Guid rolId,
            IQueryHandler<GetProfileByRolIdQuery, UsuarioProfileByRolResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetProfileByRolIdQuery(usuarioId, rolId);
            Result<UsuarioProfileByRolResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}