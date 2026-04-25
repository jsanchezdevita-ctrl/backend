using Application.Abstractions.Messaging;
using Application.Dispositivos.GetForManagement;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class GetForManagement : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dispositivos/for-management", async (
            IQueryHandler<GetDispositivosForManagementQuery, PagedResponse<DispositivoResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetDispositivosForManagementQuery(page, pageSize, search);

            Result<PagedResponse<DispositivoResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}