using Application.Abstractions.Messaging;
using Application.PuntosControl.Update;
using Domain.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PuntosControl;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Nombre,
        string Ubicacion,
        int Tipo,
        int Estado,
        string Descripcion);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("puntos-control/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdatePuntoControlCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePuntoControlCommand(
                id,
                request.Nombre,
                request.Ubicacion,
                (PuntoControlType)request.Tipo,
                (PuntoControlState)request.Estado,
                request.Descripcion);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.PuntosControl);
    }
}