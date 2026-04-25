using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Parametros.GetAll;

public sealed record GetAllParametrosQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<ParametroResponse>>;

public record ParametroResponse(
    Guid Id,
    string Llave,
    string Valor,
    string Descripcion,
    DateTime FechaActualizacion,
    string ActualizadoPor,
    int Version);