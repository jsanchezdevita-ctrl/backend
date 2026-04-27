using Application.Dispositivos.Update;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string DispositivoIdCodigo,
        string Nombre,
        int Tipo,
        string DireccionIp,
        int Estado,
        string VersionFirmware,
        Guid PuntoControlId); // Agregar PuntoControlId

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("dispositivos/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateDispositivoCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDispositivoCommand(
                id,
                request.DispositivoIdCodigo,
                request.Nombre,
                request.DireccionIp,
                request.PuntoControlId);

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