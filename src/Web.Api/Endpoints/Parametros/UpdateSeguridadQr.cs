using Application.Abstractions.Messaging;
using Application.Parametros.SeguridadQr.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class UpdateSeguridadQr : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("parametros/seguridad-qr", async (
            UpdateSeguridadQrCommand command,
            ICommandHandler<UpdateSeguridadQrCommand> handler,
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