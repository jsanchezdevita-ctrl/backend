using Application.Abstractions.Messaging;
using Application.Parametros.PoliticasAcceso.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class UpdatePoliticasAcceso : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("parametros/politicas-acceso", async (
            UpdatePoliticasAccesoCommand command,
            ICommandHandler<UpdatePoliticasAccesoCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Parametros);
    }
}