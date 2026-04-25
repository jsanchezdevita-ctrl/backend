using Application.Abstractions.Messaging;

namespace Application.Usuarios.GetProfile;

public sealed record GetUsuarioProfileQuery(Guid UsuarioId, Guid? RolId = null)
    : IQuery<UsuarioProfileResponse>;

public sealed record UsuarioProfileResponse(
    Guid UsuarioId,
    string Email,
    string NombreCompleto,
    List<RolResponse> Roles,
    bool EsAdmin,
    string NumeroDocumento,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin,
    string Estado,
    DateTime FechaRegistro,
    List<string> Zonas);

public sealed record RolResponse(
    Guid Id,
    string NombreRol,
    string Descripcion,
    bool EsAdmin);