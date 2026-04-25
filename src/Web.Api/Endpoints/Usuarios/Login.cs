using Application.Abstractions.Messaging;
using Application.Usuarios.Login;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("usuarios/login", async (
            Request request,
            ICommandHandler<LoginUsuarioCommand, LoginResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUsuarioCommand(request.Email, request.Password);

            Result<LoginResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Usuarios)
        .AllowAnonymous()
        .WithSummary("Iniciar sesión de usuario")
        .WithDescription("Autentica un usuario y devuelve tokens e información");
    }
}