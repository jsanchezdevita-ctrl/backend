using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Usuarios.GetForManagement;

public sealed record GetUsuariosForManagementQuery(
    int Page,
    int PageSize,
    string? SearchTerm)
    : IQuery<UsuariosForManagementResponse>;

public sealed record UsuariosForManagementResponse(
    PagedResponse<UsuarioConRolResponse> Usuarios,
    List<RolMetadata> Roles,
    List<StatuMetadata> Estados);

public sealed record UsuarioConRolResponse(
    Guid Id,
    string NumeroDocumento,
    string Email,
    string Nombre,
    string Apellido,
    string Estado,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin,
    DateTime FechaRegistro,
    List<RolResponse> Roles);

public sealed record RolResponse(
    Guid Id,
    string NombreRol,
    string Descripcion,
    bool EsAdmin);

public sealed record RolMetadata(
    Guid Id,
    string NombreRol,
    string Descripcion);

public sealed record StatuMetadata(
    int Value,
    string Name,
    string Description);