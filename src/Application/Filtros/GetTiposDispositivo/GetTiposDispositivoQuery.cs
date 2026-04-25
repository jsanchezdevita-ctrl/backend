using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetTiposDispositivo;

public sealed record GetTiposDispositivoQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<EnumFiltroItemResponse>>;