using Application.Abstractions.Messaging;
using Application.Filtros.GetTiposPuntoControl;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetTiposPuntoControl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/tipos-punto-control", async (
            IQueryHandler<GetTiposPuntoControlQuery, PagedResponse<EnumFiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var filtroQuery = new GetTiposPuntoControlQuery(page, pageSize, search);

            Result<PagedResponse<EnumFiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}