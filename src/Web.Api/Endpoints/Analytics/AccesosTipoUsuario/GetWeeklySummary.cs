using Application.Abstractions.Messaging;
using Application.Analytics.AccesosTipoUsuario.GetWeeklySummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Analytics.AccesosTipoUsuario;

internal sealed class GetWeeklySummary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("analytics/accesos-tipo-usuario/weekly-summary", async (
            IQueryHandler<GetAccesosTipoUsuarioWeeklySummaryQuery, List<AccesosTipoUsuarioSummaryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAccesosTipoUsuarioWeeklySummaryQuery();

            Result<List<AccesosTipoUsuarioSummaryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Analytics);
    }
}