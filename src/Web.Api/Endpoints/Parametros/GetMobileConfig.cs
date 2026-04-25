using Application.Abstractions.Messaging;
using Application.Parametros.GetMobile;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Parametros;

internal sealed class GetMobileConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("parametros/mobile", async (
            IQueryHandler<GetMobileConfigQuery, MobileConfigResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMobileConfigQuery();
            Result<MobileConfigResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                Results.Ok,
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Parametros);
    }
}