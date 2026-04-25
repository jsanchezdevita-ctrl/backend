using Application.Abstractions.Messaging;
using Application.PuntosControl.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PuntosControl;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("puntos-control/{id:guid}", async (
            Guid id,
            IQueryHandler<GetByIdPuntosControlQuery, PuntoControlResponse> handler,
            CancellationToken cancellationToken)=>
        {
            var query = new GetByIdPuntosControlQuery(id);

            Result<PuntoControlResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .AllowAnonymous()
        .WithTags(Tags.PuntosControl);
    }
}