using Application.Abstractions.Messaging;
using Application.Parametros.Autenticacion.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class UpdateAutenticacion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("parametros/autenticacion", async (
            UpdateAutenticacionCommand command,
            ICommandHandler<UpdateAutenticacionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Parametros);
    }
}