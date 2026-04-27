using Application.Abstractions.Messaging;
using Application.Filtros.GetRoles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetRoles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/roles", async (
            IQueryHandler<GetRolesQuery, PagedResponse<FiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var filtroQuery = new GetRolesQuery(page, pageSize, search);

            Result<PagedResponse<FiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}