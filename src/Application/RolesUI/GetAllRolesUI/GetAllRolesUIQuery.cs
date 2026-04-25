using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.RolesUI.GetAllRolesUI;

public sealed record GetAllRolesUIQuery(
    int Page,
    int PageSize,
    string? SearchTerm = null)
    : IQuery<PagedResponse<RolUIResponse>>;

public sealed record RolUIResponse(
    Guid RolId,
    string TipoUsuario,
    string TextColor,
    string BackgroundColor);