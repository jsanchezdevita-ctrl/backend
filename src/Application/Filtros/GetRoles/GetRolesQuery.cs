using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetRoles;

public sealed record GetRolesQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<FiltroItemResponse>>;