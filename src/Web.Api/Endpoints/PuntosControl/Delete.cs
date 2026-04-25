using Application.PuntosControl.Delete;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PuntosControl;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("puntos-control/{id:guid}", async (
            Guid id,
            ICommandHandler<DeletePuntoControlCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeletePuntoControlCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.PuntosControl);
    }
}