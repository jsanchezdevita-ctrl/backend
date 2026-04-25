using Application.Dispositivos.UpdateUltimaConexion;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class UpdateUltimaConexion : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("dispositivos/{dispositivoId:guid}/ultima-conexion", async (
            Guid dispositivoId,
            ICommandHandler<UpdateDispositivoUltimaConexionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDispositivoUltimaConexionCommand(dispositivoId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}