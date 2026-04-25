using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Usuarios.Register;

public sealed record RegisterUsuarioCommand(
    string NumeroDocumento,
    string Email,
    string Nombre,
    string Apellido,
    string Password,
    UsuarioState Estado,
    List<Guid> RolIds,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin)
    : ICommand<Guid>;
