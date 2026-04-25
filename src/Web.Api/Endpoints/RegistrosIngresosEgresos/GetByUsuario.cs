using Application.Abstractions.Messaging;
using Application.RegistrosIngresosEgresos.GetByUsuario;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.RegistrosIngresosEgresos;

internal sealed class GetByUsuario : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("registros-ingresos-egresos/usuario/{usuarioId}", async (
            Guid usuarioId,
            IQueryHandler<GetRegistrosByUsuarioQuery, List<RegistroIngresoEgresoUsuarioResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRegistrosByUsuarioQuery(usuarioId);
            Result<List<RegistroIngresoEgresoUsuarioResponse>> result =
                await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.RegistrosIngresosEgresos);
    }
}