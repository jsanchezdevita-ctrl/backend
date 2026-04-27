using Application.Abstractions.Messaging;
using Application.Dispositivos.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dispositivos/get-all", async (
            IQueryHandler<GetAllDispositivosQuery, PagedResponse<DispositivoResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllDispositivosQuery(page, pageSize, search);

            Result<PagedResponse<DispositivoResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}