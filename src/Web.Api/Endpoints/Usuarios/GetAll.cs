using Application.Abstractions.Messaging;
using Application.Usuarios.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/get-all", async (
            IQueryHandler<GetAllUsuariosQuery, PagedResponse<UsuarioConRolResponse>> handler,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 10,
            string? search = null) =>
        {
            var query = new GetAllUsuariosQuery(page, pageSize, search);

            Result<PagedResponse<UsuarioConRolResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Usuarios)
        .WithTags(Tags.Usuarios);
    }
}