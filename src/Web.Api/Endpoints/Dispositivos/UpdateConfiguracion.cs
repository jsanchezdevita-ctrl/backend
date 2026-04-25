using Application.Dispositivos.UpdateConfiguracion;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class UpdateConfiguracion : IEndpoint
{
    public sealed record Request(
        int FrecuenciaSincronizacionSegundos,
        int PotenciaTransmision,
        int CanalComunicacion);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("dispositivos/{dispositivoId:guid}/configuracion", async (
            Guid dispositivoId,
            Request request,
            ICommandHandler<UpdateDispositivoConfiguracionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDispositivoConfiguracionCommand(
                dispositivoId,
                request.FrecuenciaSincronizacionSegundos,
                (Domain.Enums.DispositivoPowerTransmission)request.PotenciaTransmision,
                request.CanalComunicacion);

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