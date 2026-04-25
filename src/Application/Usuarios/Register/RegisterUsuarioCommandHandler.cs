using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Roles;
using Domain.Usuarios;
using Domain.UsuariosRoles;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Usuarios.Register;

internal sealed class RegisterUsuarioCommandHandler(
    IApplicationDbContext context, 
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) 
    : ICommandHandler<RegisterUsuarioCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUsuarioCommand command, CancellationToken cancellationToken)
    {
        if (await context.Usuarios.AnyAsync(u => u.Email == command.Email, cancellationToken))
        {
            return Result.Failure<Guid>(UsuarioErrors.EmailNotUnique);
        }

        var rolesExistentes = await context.Roles
            .Where(r => command.RolIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        if (rolesExistentes.Count != command.RolIds.Count)
        {
            var rolNoEncontrado = command.RolIds.FirstOrDefault(id => !rolesExistentes.Contains(id));
            return Result.Failure<Guid>(RolErrors.NotFound(rolNoEncontrado));
        }

        if (command.HorarioInicio == command.HorarioFin)
        {
            return Result.Failure<Guid>(UsuarioErrors.HorarioInvalido);
        }

        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            NumeroDocumento = command.NumeroDocumento,
            Email = command.Email,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            PasswordHash = passwordHasher.Hash(String.IsNullOrEmpty(command.Password) ? "123456" : command.Password),
            Estado = command.Estado,
            FechaRegistro = dateTimeProvider.UtcNow,
            FechaUltimaModificacion = dateTimeProvider.UtcNow,
            HorarioInicio = command.HorarioInicio,
            HorarioFin = command.HorarioFin
        };


        var usuariosRoles = command.RolIds.Select(rolId => new UsuarioRol
        {
            Id = Guid.NewGuid(),
            UsuarioId = user.Id,
            RolId = rolId,
            FechaAsignacion = dateTimeProvider.UtcNow
        }).ToList();

        user.Raise(new UsuarioRegisteredDomainEvent(user.Id));

        context.Usuarios.Add(user);
        context.UsuariosRoles.AddRange(usuariosRoles);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
