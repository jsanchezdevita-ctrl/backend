using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.PuntosControl.GetById;

public sealed record GetByIdPuntosControlQuery(Guid PuntoControlId) :
    IQuery<PuntoControlResponse>;

public record PuntoControlResponse(
    Guid Id,
    string Nombre,
    string Ubicacion,
    ItemResponse<int> Tipo,
    ItemResponse<int> Estado,
    string Descripcion);