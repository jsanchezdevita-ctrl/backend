
using Application.Abstractions.Messaging;
using Application.Usuarios.Profile;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Profile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("usuarios/profile", async (
            IQueryHandler<GetUserProfileQuery, UserProfileResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserProfileQuery();
            Result<UserProfileResponse> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Usuarios);
    }
}
