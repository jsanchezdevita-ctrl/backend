using Application.Abstractions.Messaging;
using Application.Filtros.GetEstadosPuntoControl;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetEstadosPuntoControl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/estados-punto-control", async (
            IQueryHandler<GetEstadosPuntoControlQuery, PagedResponse<EnumFiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            [FromQuery(Name = "query")] string? query = null) =>
        {
            var finalSearch = search ?? query;

            var filtroQuery = new GetEstadosPuntoControlQuery(page, pageSize, finalSearch);

            Result<PagedResponse<EnumFiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}