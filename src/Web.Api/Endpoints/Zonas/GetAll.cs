using Application.Abstractions.Messaging;
using Application.Zonas.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Zonas;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("zonas/get-all", async (
            IQueryHandler<GetAllZonasQuery, PagedResponse<ZonaResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllZonasQuery(page, pageSize, search);

            Result<PagedResponse<ZonaResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Zonas);
    }
}