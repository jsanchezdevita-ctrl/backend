using Application.Abstractions.Messaging;
using Application.Zonas.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Zonas;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Nombre,
        string Descripcion,
        List<ZonaRolRequest> Roles,
        List<Guid> PuntoControlIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("zonas/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateZonaCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateZonaCommand(
                id,
                request.Nombre,
                request.Descripcion,
                request.Roles,
                request.PuntoControlIds);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Zonas);
    }
}