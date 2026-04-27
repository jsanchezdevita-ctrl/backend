using Application.Abstractions.Messaging;
using Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Dashboard;

internal sealed class GetDisponibilidad : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dashboard/get-disponibilidad", async (
            Guid zonaId,
            IQueryHandler<GetDisponibilidadPorRolesQuery, DisponibilidadPorRolesResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDisponibilidadPorRolesQuery(zonaId);

            Result<DisponibilidadPorRolesResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .AllowAnonymous()
        .WithTags(Tags.Dashboard);
    }
}