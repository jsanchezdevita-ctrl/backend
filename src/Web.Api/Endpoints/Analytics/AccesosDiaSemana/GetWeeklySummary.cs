using Application.Abstractions.Messaging;
using Application.Analytics.AccesosDiaSemana.GetWeeklySummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Analytics.AccesosDiaSemana;

internal sealed class GetWeeklySummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("analytics/accesos-dia-semana/weekly-summary", async (
            IQueryHandler<GetAccesosDiaSemanaWeeklySummaryQuery, List<AccesosDiaSemanaSummaryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAccesosDiaSemanaWeeklySummaryQuery();

            Result<List<AccesosDiaSemanaSummaryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Analytics);
    }
}