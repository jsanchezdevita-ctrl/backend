using Application.Parametros.GetAll;
using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("parametros/get-all", async (
            IQueryHandler<GetAllParametrosQuery, PagedResponse<ParametroResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllParametrosQuery(page, pageSize, search);

            Result<PagedResponse<ParametroResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Parametros);
    }
}