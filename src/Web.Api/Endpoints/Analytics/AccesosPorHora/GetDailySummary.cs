using Application.Abstractions.Messaging;
using Application.Analytics.AccesosPorHora.GetDailySummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Analytics.AccesosPorHora;

internal sealed class GetDailySummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("analytics/accesos-por-hora/daily-summary", async (
            IQueryHandler<GetAccesosPorHoraDailySummaryQuery, List<AccesosPorHoraDailyResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAccesosPorHoraDailySummaryQuery();

            Result<List<AccesosPorHoraDailyResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Analytics);
    }
}