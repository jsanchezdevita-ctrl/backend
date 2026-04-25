using Application.Abstractions.Messaging;

namespace Application.Usuarios.GetMetadata;

public sealed record GetUsuariosMetadataQuery : IQuery<UsuariosMetadataResponse>;

public sealed record UsuariosMetadataResponse(
    List<RolMetadata> Roles,
    List<StatuMetadata> Estados);

public sealed record RolMetadata(
    Guid Id,
    string NombreRol,
    string Descripcion);

public sealed record StatuMetadata(
    int Value,
    string Name,
    string Description);