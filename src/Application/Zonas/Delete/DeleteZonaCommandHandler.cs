using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.Delete;

internal sealed class DeleteZonaCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : ICommandHandler<DeleteZonaCommand>
{
    public async Task<Result> Handle(
        DeleteZonaCommand command,
        CancellationToken cancellationToken)
    {
        var zona = await context.Zonas
            .FirstOrDefaultAsync(z => z.Id == command.ZonaId, cancellationToken);

        if (zona is null)
        {
            return Result.Failure(ZonaErrors.NotFound(command.ZonaId));
        }

        // Verificar si tiene ZonaRoles con espacio utilizado
        var zonaRolesConEspacio = await context.ZonasRoles
            .Where(zr => zr.ZonaId == command.ZonaId && zr.EspacioUtilizado > 0)
            .AnyAsync(cancellationToken);

        if (zonaRolesConEspacio)
        {
            return Result.Failure(ZonaErrors.CannotDeleteWithEspacioUtilizado(command.ZonaId));
        }

        // Eliminar relaciones primero (CASCADE en BD o manualmente)

        // Eliminar ZonaPuntosControl
        var zonasPuntosControl = await context.ZonasPuntosControl
            .Where(zpc => zpc.ZonaId == command.ZonaId)
            .ToListAsync(cancellationToken);
        context.ZonasPuntosControl.RemoveRange(zonasPuntosControl);

        // Eliminar ZonaRoles (ya verificamos que no tienen espacio utilizado)
        var zonasRoles = await context.ZonasRoles
            .Where(zr => zr.ZonaId == command.ZonaId)
            .ToListAsync(cancellationToken);
        context.ZonasRoles.RemoveRange(zonasRoles);

        // Finalmente eliminar la zona
        zona.SoftDelete(userContext.Email);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}