using Application.Abstractions.Messaging;
using Application.Zonas.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Zonas;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("zonas/{id:guid}", async (
            Guid id,
            ICommandHandler<DeleteZonaCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteZonaCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Zonas);
    }
}