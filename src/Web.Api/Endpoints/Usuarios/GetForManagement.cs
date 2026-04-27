using Application.Abstractions.Messaging;
using Application.Usuarios.GetForManagement;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetForManagement : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/for-management", async (
            IQueryHandler<GetUsuariosForManagementQuery, UsuariosForManagementResponse> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetUsuariosForManagementQuery(page, pageSize, search);

            Result<UsuariosForManagementResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        ////.RequireAuthorization()
        ////.HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}