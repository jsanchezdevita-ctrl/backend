using Application.Abstractions.Messaging;

namespace Application.Usuarios.Profile;

public record GetUserProfileQuery : IQuery<UserProfileResponse>{}

public record UserProfileResponse(
    Guid UsuarioId,
    string NombreCompleto,
    string Email,
    string Rol,
    bool EsAdmin);