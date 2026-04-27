using Application.RegistrosIngresosEgresos.GetForMonitoring;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.RegistrosIngresosEgresos;

internal sealed class GetForMonitoring : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("registros-ingresos-egresos/monitoring", async (
            IQueryHandler<GetRegistrosForMonitoringQuery, PagedResponse<RegistroIngresoEgresoMonitoringResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            Guid? estadoRegistroId = null,
            Guid? tipoUsuarioId = null,
            Guid? puntoAccesoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null) =>
        {
            var query = new GetRegistrosForMonitoringQuery(
                page, 
                pageSize, 
                search, 
                estadoRegistroId, 
                tipoUsuarioId,
                puntoAccesoId, 
                fechaDesde, 
                fechaHasta);

            Result<PagedResponse<RegistroIngresoEgresoMonitoringResponse>> result = 
            await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.RegistrosIngresosEgresos);
    }
}