using Application.Abstractions.Messaging;
using Application.Filtros.GetTiposUsuario;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Filtros;

internal sealed class GetTiposUsuario : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("filtros/tipos-usuario", async (
            IQueryHandler<GetTiposUsuarioQuery, PagedResponse<FiltroItemResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var filtroQuery = new GetTiposUsuarioQuery(page, pageSize, search);

            Result<PagedResponse<FiltroItemResponse>> result = await handler.Handle(filtroQuery, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Filtros);
    }
}