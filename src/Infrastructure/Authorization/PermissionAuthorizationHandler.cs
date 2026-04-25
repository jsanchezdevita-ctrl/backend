using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return;
        }

        //// TODO: You definitely want to reject unauthenticated users here.
        //if (context.User is { Identity.IsAuthenticated: true })
        //{
        //    // TODO: Remove this call when you implement the PermissionProvider.GetForUserIdAsync
        //    context.Succeed(requirement);

        //    return;
        //}

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        PermissionProvider permissionProvider = scope.ServiceProvider.GetRequiredService<PermissionProvider>();

        Guid usuarioId = context.User.GetUsuarioId();

        HashSet<string> permissions = await permissionProvider.GetForUsuarioIdAsync(usuarioId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
