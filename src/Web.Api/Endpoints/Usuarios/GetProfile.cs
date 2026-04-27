using Application.Abstractions.Messaging;
using Application.Usuarios.GetProfile;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/{usuarioId}/profile", async (
            Guid usuarioId,
            Guid? rolId,
            IQueryHandler<GetUsuarioProfileQuery, UsuarioProfileResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsuarioProfileQuery(usuarioId, rolId);
            Result<UsuarioProfileResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}