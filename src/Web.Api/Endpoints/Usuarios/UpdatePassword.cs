using Application.Abstractions.Messaging;
using Application.Usuarios.UpdatePassword;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class UpdatePassword : IEndpoint
{
    public sealed record Request(string NewPassword);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("usuarios/{id}/password", async (
            Guid id,
            Request request,
            ICommandHandler<UpdatePasswordCommand> handler,
            System.Security.Claims.ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePasswordCommand(
                id,
                request.NewPassword);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}