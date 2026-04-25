using Application.Abstractions.Messaging;
using Application.QrTokens.Validate;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.QrTokens;

internal sealed class RegisterQrAccess : IEndpoint
{
    public sealed record Request(
        string QrToken,
        string DispositivoId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("qr-tokens/register-access", async (
            ICommandHandler<ValidateQrTokenCommand, ValidateQrTokenResponse> handler,
            Request request,
            CancellationToken cancellationToken) =>
        {
            var command = new ValidateQrTokenCommand(
                request.QrToken,
                request.DispositivoId);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                validateQrTokenResponse => Results.Ok(validateQrTokenResponse),
                CustomResults.Problem);
        })
        //.RequireAuthorization()
        //.HasPermission(Permissions.Admin)
        .WithTags(Tags.QrTokens);
    }
}