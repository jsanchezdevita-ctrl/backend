using Application.Abstractions.Messaging;
using Application.RolesUI.GetAllRolesUI;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Roles;

internal sealed class GetAllRolesUI : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("roles/get-all-ui", async (
            IQueryHandler<GetAllRolesUIQuery, PagedResponse<RolUIResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllRolesUIQuery(page, pageSize, search);

            Result<PagedResponse<RolUIResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.RolesUI);
    }
}