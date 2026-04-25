using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.PuntosControl.GetForManagement;

public sealed record GetPuntosControlForManagementQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PuntosControlForManagementResponse>;

public record PuntosControlForManagementResponse(
    PagedResponse<PuntoControlConRolResponse> PuntosControl,
    List<RolMetadata> Roles,
    List<TipoMetadata> Tipos,
    List<EstadoMetadata> Estados);

public record PuntoControlConRolResponse(
    Guid Id,
    string Nombre,
    string Ubicacion,
    string Tipo,
    string Estado,
    string Descripcion);

public record RolMetadata(Guid Id, string NombreRol, string Descripcion);
public record TipoMetadata(int Id, string Nombre, string Descripcion);
public record EstadoMetadata(int Id, string Nombre, string Descripcion);