using Application.RegistrosIngresosEgresos.GetSummaryMonitoring;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.RegistrosIngresosEgresos;

internal sealed class GetSummaryMonitoring : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("registros-ingresos-egresos/summary-monitoring", async (
            IQueryHandler<GetRegistrosSummaryMonitoringQuery, RegistrosSummaryMonitoringResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRegistrosSummaryMonitoringQuery();

            Result<RegistrosSummaryMonitoringResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.RegistrosIngresosEgresos);
    }
}