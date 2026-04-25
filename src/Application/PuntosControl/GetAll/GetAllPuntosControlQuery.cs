using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.PuntosControl.GetAll;

public sealed record GetAllPuntosControlQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<PuntoControlResponse>>;

public record PuntoControlResponse(
    Guid Id,
    string Nombre,
    string Ubicacion,
    string Tipo,
    string Estado,
    string Descripcion);