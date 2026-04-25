using Application.Abstractions.Messaging;
using Application.Usuarios.Logout;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Logout : IEndpoint
{
    public sealed record Request(string RefreshToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("usuarios/logout", async (
            //Request request,
            ICommandHandler<LogoutCommand, Unit> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LogoutCommand(/*request.RefreshToken*/);

            Result<Unit> result = await handler.Handle(command, cancellationToken);

            return result.Match(_ => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.Usuarios);
    }
}
