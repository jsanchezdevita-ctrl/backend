using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetEstadosPuntoControl;

public sealed record GetEstadosPuntoControlQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<EnumFiltroItemResponse>>;