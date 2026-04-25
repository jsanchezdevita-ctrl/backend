using Application.Abstractions.Messaging;
using Application.Dispositivos.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dispositivos;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dispositivos/{id:guid}", async (
            Guid id,
            IQueryHandler<GetDispositivoByIdQuery, DispositivoResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDispositivoByIdQuery(id);
            Result<DispositivoResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.Dispositivos);
    }
}