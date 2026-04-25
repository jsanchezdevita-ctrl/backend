using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetDispositivos;

public sealed record GetDispositivosQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<FiltroItemResponse>>;