using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Usuarios.GetAll;

public sealed record GetAllUsuariosQuery(
    int Page,
    int PageSize,
    string? SearchTerm)
    : IQuery<PagedResponse<UsuarioConRolResponse>>;

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