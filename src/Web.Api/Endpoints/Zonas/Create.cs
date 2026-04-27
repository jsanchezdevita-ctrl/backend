using Application.Abstractions.Messaging;
using Application.Zonas.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Zonas;

internal sealed class Create : IEndpoint
{
    public sealed record Request(
        string Nombre,
        string Descripcion,
        List<ZonaRolRequest> Roles,
        List<Guid> PuntoControlIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("zonas", async (
            ICommandHandler<CreateZonaCommand, Guid> handler,
            Request request,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateZonaCommand(
                request.Nombre,
                request.Descripcion,
                request.Roles,
                request.PuntoControlIds);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                id => Results.Created($"/zonas/{id}", id),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Zonas);
    }
}