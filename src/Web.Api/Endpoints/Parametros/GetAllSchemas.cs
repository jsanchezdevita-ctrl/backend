using Application.Abstractions.Messaging;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Parametros.GetAllSchemas;

namespace Web.Api.Endpoints.Parametros;

internal sealed class GetAllSchemas : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("parametros/schemas", async (
            IQueryHandler<GetAllParametroSchemasQuery, PagedResponse<ParametroSchemaResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllParametroSchemasQuery(page, pageSize, search);

            Result<PagedResponse<ParametroSchemaResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Parametros);
    }
}