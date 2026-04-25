using SharedKernel;

namespace Application.Usuarios.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    Guid UsuarioId,
    string Email,
    string NombreCompleto,
    List<string> Roles,
    List<ItemResponse<Guid>> RolesInfo,
    bool EsAdmin,
    string NumeroDocumento);