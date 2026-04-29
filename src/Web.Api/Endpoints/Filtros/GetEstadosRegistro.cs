using Application.Abstractions.Messaging;
using Application.Filtros.GetEstadosRegistro;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetEstadosRegistro : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/estados-registro", async (
            IQueryHandler<GetEstadosRegistroQuery, PagedResponse<FiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            [FromQuery(Name = "query")] string? query = null) =>
        {
            var finalSearch = search ?? query;

            var filtroQuery = new GetEstadosRegistroQuery(page, pageSize, finalSearch);

            Result<PagedResponse<FiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}