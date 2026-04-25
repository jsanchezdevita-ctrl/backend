using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Usuarios.Logout;

internal sealed class LogoutCommandHandler(
    IApplicationDbContext context,
    ICookieService cookieService)
    : ICommandHandler<LogoutCommand, Unit>
{
    public async Task<Result<Unit>> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        //var refreshToken = await context.RefreshTokens
        //    .FirstOrDefaultAsync(x => x.Token == command.RefreshToken, cancellationToken);

        //if (refreshToken is null || refreshToken.IsRevoked)
        //{
        //    return Result.Success(Unit.Value);
        //}

        //refreshToken.Revoke();

        //context.RefreshTokens.Update(refreshToken);

        cookieService.DeleteTokenCookie();

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(Unit.Value);
    }
}
