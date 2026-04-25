using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetEstadosRegistro;

public sealed record GetEstadosRegistroQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<FiltroItemResponse>>;