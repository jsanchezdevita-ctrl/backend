using Application.Abstractions.Messaging;
using Application.Usuarios.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("usuarios/{usuarioId}", async (
            Guid usuarioId,
            ICommandHandler<DeleteUsuarioCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteUsuarioCommand(usuarioId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}