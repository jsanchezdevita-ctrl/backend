using Application.Abstractions.Messaging;
using Application.QrTokens.Generate;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.QrTokens;

internal sealed class GenerateQrToken : IEndpoint
{
    public sealed record Request(Guid UsuarioId, Guid RolId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("qr-tokens/generate", async (
            ICommandHandler<GenerateQrTokenCommand, QrTokenResponse> handler,
            Request request,
            CancellationToken cancellationToken) =>
        {
            var command = new GenerateQrTokenCommand(request.UsuarioId, request.RolId);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                tokenResponse => Results.Ok(tokenResponse),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.QrTokens);
    }
}