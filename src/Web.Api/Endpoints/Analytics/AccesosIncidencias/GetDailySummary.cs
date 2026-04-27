using Application.Abstractions.Messaging;
using Application.Analytics.AccesosIncidencias.GetDailySummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Analytics.AccesosIncidencias;

internal sealed class GetDailySummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("analytics/accesos-incidencias/daily-summary", async (
            IQueryHandler<GetAccesosIncidenciasDailySummaryQuery, List<AccesoIncidenciaDailyResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAccesosIncidenciasDailySummaryQuery();

            Result<List<AccesoIncidenciaDailyResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Analytics);
    }
}