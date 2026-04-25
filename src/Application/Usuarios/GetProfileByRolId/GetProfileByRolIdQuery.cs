using Application.Abstractions.Messaging;

namespace Application.Usuarios.GetProfileByRolId;

public sealed record GetProfileByRolIdQuery(Guid UsuarioId, Guid RolId)
    : IQuery<UsuarioProfileByRolResponse>;

public sealed record UsuarioProfileByRolResponse(
    Guid UsuarioId,
    string Email,
    string NombreCompleto,
    string Rol,
    bool EsAdmin,
    string NumeroDocumento,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin,
    string Estado,
    DateTime FechaRegistro,
    List<string> Zonas);