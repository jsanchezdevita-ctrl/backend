using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetTiposUsuario;

public sealed record GetTiposUsuarioQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<FiltroItemResponse>>;