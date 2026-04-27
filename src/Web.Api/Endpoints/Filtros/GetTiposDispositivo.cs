using Application.Abstractions.Messaging;
using Application.Filtros.GetTiposDispositivo;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetTiposDispositivo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/tipos-dispositivo", async (
            IQueryHandler<GetTiposDispositivoQuery, PagedResponse<EnumFiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var filtroQuery = new GetTiposDispositivoQuery(page, pageSize, search);

            Result<PagedResponse<EnumFiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}