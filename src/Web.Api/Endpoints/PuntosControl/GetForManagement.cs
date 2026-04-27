using Application.Abstractions.Messaging;
using Application.PuntosControl.GetForManagement;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PuntosControl;

internal sealed class GetForManagement : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("puntos-control/for-management", async (
            IQueryHandler<GetPuntosControlForManagementQuery, PuntosControlForManagementResponse> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetPuntosControlForManagementQuery(page, pageSize, search);

            Result<PuntosControlForManagementResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.PuntosControl);
    }
}