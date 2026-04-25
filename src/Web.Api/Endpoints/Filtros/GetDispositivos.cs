
using Application.Abstractions.Messaging;
using Application.Filtros.GetDispositivos;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetDispositivos : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/dispositivos", async (
            IQueryHandler<GetDispositivosQuery, PagedResponse<FiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var filtroQuery = new GetDispositivosQuery(page, pageSize, search);

            Result<PagedResponse<FiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .AllowAnonymous()
        .WithTags(Tags.Filtros);
    }
}