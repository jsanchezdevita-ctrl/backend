using Application.Abstractions.Messaging;
using Application.Usuarios.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/{usuarioId}", async (
            Guid usuarioId,
            IQueryHandler<GetUsuarioByIdQuery, UsuarioByIdResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsuarioByIdQuery(usuarioId);
            Result<UsuarioByIdResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasAnyPermission(Permissions.Admin, Permissions.Usuarios)
        .WithTags(Tags.Usuarios);
    }
}