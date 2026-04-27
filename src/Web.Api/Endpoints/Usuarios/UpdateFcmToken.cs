using Application.Abstractions.Messaging;
using Application.Usuarios.UpdateFcmToken;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class UpdateFcmToken : IEndpoint
{
    public sealed record Request(string FcmToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("usuarios/me/fcm-token", async (
            Request request,
            ICommandHandler<UpdateUsuarioFcmTokenCommand> handler,
            System.Security.Claims.ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateUsuarioFcmTokenCommand(request.FcmToken);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.Ok(new
                {
                    Message = "Token FCM actualizado exitosamente",
                    UpdatedAt = DateTime.UtcNow
                }),
                CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}