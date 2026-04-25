using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Usuarios.Update;

public sealed record UpdateUsuarioCommand(
    Guid UsuarioId,
    string NumeroDocumento,
    string Email,           
    string Nombre,
    string Apellido,
    UsuarioState Estado,
    List<Guid> RolIds,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin) : ICommand;