using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Zonas;
using Domain.ZonasRoles;
using Domain.ZonasPuntosControl;
using Domain.Roles;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Zonas.Create;

internal sealed class CreateZonaCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateZonaCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateZonaCommand command,
        CancellationToken cancellationToken)
    {
        if (await context.Zonas.AnyAsync(
            z => z.Nombre == command.Nombre, cancellationToken))
        {
            return Result.Failure<Guid>(ZonaErrors.NameNotUnique);
        }

        var rolIds = command.Roles.Select(r => r.RolId).ToList();
        var rolesExistentes = await context.Roles
            .Where(r => rolIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (rolesExistentes.Count != rolIds.Count)
        {
            var rolNoEncontrado = rolIds.FirstOrDefault(id => !rolesExistentes.Contains(id));
            return Result.Failure<Guid>(RolErrors.NotFound(rolNoEncontrado));
        }

        foreach (var rolRequest in command.Roles)
        {
            if (rolRequest.CapacidadMaxima <= 0)
            {
                return Result.Failure<Guid>(ZonaRolErrors.CapacidadNoValida);
            }
        }

        var puntosControlExistentes = await context.PuntosControl
            .Where(pc => command.PuntoControlIds.Contains(pc.Id))
            .Select(pc => pc.Id)
            .ToListAsync(cancellationToken);

        if (puntosControlExistentes.Count != command.PuntoControlIds.Count)
        {
            var pcNoEncontrado = command.PuntoControlIds
                .FirstOrDefault(id => !puntosControlExistentes.Contains(id));
            return Result.Failure<Guid>(PuntoControlErrors.NotFound(pcNoEncontrado));
        }

        var zona = new Zona
        {
            Id = Guid.NewGuid(),
            Nombre = command.Nombre,
            Descripcion = command.Descripcion
        };

        var zonasRoles = command.Roles.Select(rolRequest => new ZonaRol
        {
            Id = Guid.NewGuid(),
            ZonaId = zona.Id,
            RolId = rolRequest.RolId,
            CapacidadMaxima = rolRequest.CapacidadMaxima,
            EspacioUtilizado = 0
        }).ToList();

        var zonasPuntosControl = command.PuntoControlIds.Select(pcId => new ZonaPuntoControl
        {
            Id = Guid.NewGuid(),
            ZonaId = zona.Id,
            PuntoControlId = pcId
        }).ToList();

        context.Zonas.Add(zona);
        context.ZonasRoles.AddRange(zonasRoles);
        context.ZonasPuntosControl.AddRange(zonasPuntosControl);

        await context.SaveChangesAsync(cancellationToken);

        return zona.Id;
    }
}