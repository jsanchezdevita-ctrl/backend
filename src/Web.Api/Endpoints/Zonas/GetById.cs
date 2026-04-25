using Application.Abstractions.Messaging;
using Application.Zonas.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Zonas;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("zonas/{id:guid}", async (
            Guid id,
            IQueryHandler<GetZonaByIdQuery, ZonaResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetZonaByIdQuery(id);
            Result<ZonaResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Zonas);
    }
}