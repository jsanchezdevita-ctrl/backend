using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetEstadosUsuario;

public sealed record GetEstadosUsuarioQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<EnumFiltroItemResponse>>;