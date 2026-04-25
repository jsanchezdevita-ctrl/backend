using Application.Abstractions.Messaging;
using Application.Analytics.AccesosResumen.GetSummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Analytics.AccesosResumen;

internal sealed class GetSummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("analytics/accesos-resumen/summary", async (
            IQueryHandler<GetAccesosResumenSummaryQuery, AccesosResumenSummaryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAccesosResumenSummaryQuery();

            Result<AccesosResumenSummaryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Analytics);
    }
}