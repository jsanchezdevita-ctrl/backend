using Application.Abstractions.Messaging;
using Application.PuntosControl.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PuntosControl;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("puntos-control/get-all", async (
            IQueryHandler<GetAllPuntosControlQuery, PagedResponse<PuntoControlResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllPuntosControlQuery(page, pageSize, search);

            Result<PagedResponse<PuntoControlResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.PuntosControl);
    }
}