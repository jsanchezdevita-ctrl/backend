using Application.Dispositivos.Delete;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("dispositivos/{id:guid}", async (
            Guid id,
            ICommandHandler<DeleteDispositivoCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteDispositivoCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}