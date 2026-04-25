using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Zonas;
using Domain.ZonasRoles;
using Domain.ZonasPuntosControl;
using Domain.Roles;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.Update;

internal sealed class UpdateZonaCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateZonaCommand>
{
    public async Task<Result> Handle(
        UpdateZonaCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validar que la zona existe
        var zona = await context.Zonas
            .FirstOrDefaultAsync(z => z.Id == command.ZonaId, cancellationToken);

        if (zona is null)
        {
            return Result.Failure(ZonaErrors.NotFound(command.ZonaId));
        }

        // 2. Validar nombre único (excluyendo la zona actual)
        if (await context.Zonas.AnyAsync(
            z => z.Nombre == command.Nombre && z.Id != command.ZonaId, cancellationToken))
        {
            return Result.Failure(ZonaErrors.NameNotUnique);
        }

        // 3. Validar que todos los roles existan
        var rolIds = command.Roles.Select(r => r.RolId).ToList();
        var rolesExistentes = await context.Roles
            .Where(r => rolIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (rolesExistentes.Count != rolIds.Count)
        {
            var rolNoEncontrado = rolIds.FirstOrDefault(id => !rolesExistentes.Contains(id));
            return Result.Failure(RolErrors.NotFound(rolNoEncontrado));
        }

        // 4. Validar capacidades de roles
        foreach (var rolRequest in command.Roles)
        {
            if (rolRequest.CapacidadMaxima <= 0)
            {
                return Result.Failure(ZonaRolErrors.CapacidadNoValida);
            }
        }

        // 5. Validar que todos los puntos de control existan
        var puntosControlExistentes = await context.PuntosControl
            .Where(pc => command.PuntoControlIds.Contains(pc.Id))
            .Select(pc => pc.Id)
            .ToListAsync(cancellationToken);

        if (puntosControlExistentes.Count != command.PuntoControlIds.Count)
        {
            var pcNoEncontrado = command.PuntoControlIds
                .FirstOrDefault(id => !puntosControlExistentes.Contains(id));
            return Result.Failure(PuntoControlErrors.NotFound(pcNoEncontrado));
        }

        // 6. Actualizar datos básicos de la zona
        zona.Nombre = command.Nombre;
        zona.Descripcion = command.Descripcion;

        // 7. Sincronizar ZonaRoles
        await SincronizarZonaRolesAsync(zona.Id, command.Roles, cancellationToken);

        // 8. Sincronizar ZonaPuntosControl
        await SincronizarZonaPuntosControlAsync(zona.Id, command.PuntoControlIds, cancellationToken);

        // 9. Guardar cambios
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task SincronizarZonaRolesAsync(
        Guid zonaId,
        List<ZonaRolRequest> nuevosRoles,
        CancellationToken cancellationToken)
    {
        // Obtener relaciones actuales
        var relacionesActuales = await context.ZonasRoles
            .Where(zr => zr.ZonaId == zonaId)
            .ToListAsync(cancellationToken);

        // Eliminar relaciones que ya no están en la nueva lista
        var rolesAEliminar = relacionesActuales
            .Where(ra => !nuevosRoles.Any(nr => nr.RolId == ra.RolId))
            .ToList();

        // Validar que no se eliminen relaciones con espacio utilizado
        foreach (var relacionEliminar in rolesAEliminar)
        {
            if (relacionEliminar.EspacioUtilizado > 0)
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar la relación con el rol {relacionEliminar.RolId} porque tiene espacio utilizado");
            }
            context.ZonasRoles.Remove(relacionEliminar);
        }

        // Actualizar capacidades de relaciones existentes
        foreach (var relacionActual in relacionesActuales)
        {
            var nuevoRol = nuevosRoles.FirstOrDefault(nr => nr.RolId == relacionActual.RolId);
            if (nuevoRol != null)
            {
                // Validar que la nueva capacidad no sea menor al espacio utilizado
                if (nuevoRol.CapacidadMaxima < relacionActual.EspacioUtilizado)
                {
                    throw new InvalidOperationException(
                        $"La nueva capacidad ({nuevoRol.CapacidadMaxima}) no puede ser menor al espacio utilizado ({relacionActual.EspacioUtilizado})");
                }
                relacionActual.CapacidadMaxima = nuevoRol.CapacidadMaxima;
            }
        }

        // Agregar nuevas relaciones
        var rolIdsActuales = relacionesActuales.Select(ra => ra.RolId).ToList();
        var relacionesAAgregar = nuevosRoles
            .Where(nr => !rolIdsActuales.Contains(nr.RolId))
            .Select(nr => new ZonaRol
            {
                Id = Guid.NewGuid(),
                ZonaId = zonaId,
                RolId = nr.RolId,
                CapacidadMaxima = nr.CapacidadMaxima,
                EspacioUtilizado = 0
            });

        await context.ZonasRoles.AddRangeAsync(relacionesAAgregar, cancellationToken);
    }

    private async Task SincronizarZonaPuntosControlAsync(
        Guid zonaId,
        List<Guid> nuevosPuntoControlIds,
        CancellationToken cancellationToken)
    {
        // Obtener relaciones actuales
        var relacionesActuales = await context.ZonasPuntosControl
            .Where(zpc => zpc.ZonaId == zonaId)
            .ToListAsync(cancellationToken);

        // Eliminar relaciones que ya no están en la nueva lista
        var relacionesAEliminar = relacionesActuales
            .Where(ra => !nuevosPuntoControlIds.Contains(ra.PuntoControlId))
            .ToList();

        context.ZonasPuntosControl.RemoveRange(relacionesAEliminar);

        // Agregar nuevas relaciones
        var puntoControlIdsActuales = relacionesActuales.Select(ra => ra.PuntoControlId).ToList();
        var relacionesAAgregar = nuevosPuntoControlIds
            .Where(id => !puntoControlIdsActuales.Contains(id))
            .Select(pcId => new ZonaPuntoControl
            {
                Id = Guid.NewGuid(),
                ZonaId = zonaId,
                PuntoControlId = pcId
            });

        await context.ZonasPuntosControl.AddRangeAsync(relacionesAAgregar, cancellationToken);
    }
}