using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetZonas;

public sealed record GetZonasQuery(
int Page,
int PageSize,
string? SearchTerm) : IQuery<PagedResponse<FiltroItemResponse>>;

