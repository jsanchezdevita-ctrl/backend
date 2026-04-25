using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetTiposPuntoControl;

public sealed record GetTiposPuntoControlQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<EnumFiltroItemResponse>>;