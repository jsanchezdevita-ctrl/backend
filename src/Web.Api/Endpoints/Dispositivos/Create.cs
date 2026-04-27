using Application.Dispositivos.Create;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class Create : IEndpoint
{
    public sealed record Request(
        string DispositivoId,
        string Nombre,
        int Tipo,
        string DireccionIp,
        string VersionFirmware,
        Guid PuntoControlId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("dispositivos", async (
            Request request,
            ICommandHandler<CreateDispositivoCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateDispositivoCommand(
                request.DispositivoId,
                request.Nombre,
                request.DireccionIp,
                request.PuntoControlId);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                id => Results.Created($"/dispositivos/{id}", id),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}