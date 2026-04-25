using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Filtros.GetPuntosControl;
public sealed record GetPuntosControlQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<FiltroPuntoControl>>;



public record FiltroPuntoControl(
    Guid Value,
    string Label,
    Guid DispositivoId,
    string NombreDispositivo);