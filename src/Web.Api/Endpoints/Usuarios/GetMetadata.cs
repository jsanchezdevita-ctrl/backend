using Application.Abstractions.Messaging;
using Application.Usuarios.GetMetadata;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetMetadata : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/metadata", async (
            IQueryHandler<GetUsuariosMetadataQuery, UsuariosMetadataResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsuariosMetadataQuery();

            Result<UsuariosMetadataResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}