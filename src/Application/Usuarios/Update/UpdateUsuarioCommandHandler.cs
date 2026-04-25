using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Roles;
using Domain.Usuarios;
using Domain.UsuariosRoles;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.Update;

internal sealed class UpdateUsuarioCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<UpdateUsuarioCommand>
{
    public async Task<Result> Handle(UpdateUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == command.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            return Result.Failure(UsuarioErrors.NotFound(command.UsuarioId));
        }

        if (usuario.Id == userContext.UsuarioId && command.Estado == UsuarioState.Restringido)
        {
            return Result.Failure(UsuarioErrors.CannotDeactivateSelf);
        }

        if (usuario.Email != command.Email &&
            await context.Usuarios.AnyAsync(u => u.Email == command.Email, cancellationToken))
        {
            return Result.Failure(UsuarioErrors.EmailNotUnique);
        }

        var rolesExistentes = await context.Roles
            .Where(r => command.RolIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (rolesExistentes.Count != command.RolIds.Count)
        {
            var rolNoEncontrado = command.RolIds.FirstOrDefault(id => !rolesExistentes.Contains(id));
            return Result.Failure(RolErrors.NotFound(rolNoEncontrado));
        }

        if (command.HorarioInicio == command.HorarioFin)
        {
            return Result.Failure(UsuarioErrors.HorarioInvalido);
        }

        var rolesActuales = await context.UsuariosRoles
            .Where(ur => ur.UsuarioId == usuario.Id)
            .ToListAsync(cancellationToken);

        usuario.NumeroDocumento = command.NumeroDocumento;
        usuario.Email = command.Email;
        usuario.Nombre = command.Nombre;
        usuario.Apellido = command.Apellido;
        usuario.Estado = command.Estado;
        usuario.FechaUltimaModificacion = dateTimeProvider.UtcNow;
        usuario.HorarioInicio = command.HorarioInicio;
        usuario.HorarioFin = command.HorarioFin;

        await SincronizarRolesUsuarioAsync(usuario.Id, command.RolIds, rolesActuales, cancellationToken);

        usuario.Raise(new UsuarioUpdateDomainEvent(usuario.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task SincronizarRolesUsuarioAsync(
        Guid usuarioId,
        List<Guid> nuevosRolIds,
        List<UsuarioRol> rolesActuales,
        CancellationToken cancellationToken)
    {
        var rolesAEliminar = rolesActuales
            .Where(ra => !nuevosRolIds.Contains(ra.RolId))
            .ToList();

        foreach (var rolEliminar in rolesAEliminar)
        {
            context.UsuariosRoles.Remove(rolEliminar);
        }

        var rolIdsActuales = rolesActuales.Select(ra => ra.RolId).ToList();

        var rolesAAgregar = nuevosRolIds
            .Where(nuevoId => !rolIdsActuales.Contains(nuevoId))
            .Select(rolId => new UsuarioRol
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                RolId = rolId,
                FechaAsignacion = dateTimeProvider.UtcNow
            });

        await context.UsuariosRoles.AddRangeAsync(rolesAAgregar, cancellationToken);
    }
}