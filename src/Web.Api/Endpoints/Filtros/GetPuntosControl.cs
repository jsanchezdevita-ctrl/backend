using Application.Abstractions.Messaging;
using Application.Filtros.GetPuntosControl;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetPuntosControl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/puntos-control", async (
            IQueryHandler<GetPuntosControlQuery, PagedResponse<FiltroPuntoControl>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            [FromQuery(Name = "query")] string? query = null) =>
        {
            var finalSearch = search ?? query;

            var filtroQuery = new GetPuntosControlQuery(page, pageSize, finalSearch);

            Result<PagedResponse<FiltroPuntoControl>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}