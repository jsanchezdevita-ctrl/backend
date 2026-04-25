using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Usuarios.Login;
using Application.Usuarios.RefreshToken;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class RefreshToken : IEndpoint
{
    public sealed record Request(string RefreshToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("usuarios/refresh-token", async (
            Request request,
            ICommandHandler<RefreshTokenCommand, LoginResponse> handler,
            ICookieService? cookieService,
            CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommand(request.RefreshToken);

            Result<LoginResponse> result = await handler.Handle(command, cancellationToken);
            
            if (result.IsSuccess && cookieService != null)
            {
                cookieService.SetTokenCookie(result.Value.AccessToken);
            }

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Usuarios);
    }
}
