using Application.Abstractions.Messaging;
using Application.Parametros.ConfiguracionAdicional.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class UpdateConfiguracionAdicional : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("parametros/configuracion-adicional", async (
            UpdateConfiguracionAdicionalCommand command,
            ICommandHandler<UpdateConfiguracionAdicionalCommand> handler,
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