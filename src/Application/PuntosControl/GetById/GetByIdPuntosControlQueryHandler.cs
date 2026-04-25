using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.PuntosControl.GetById;

internal sealed class GetByIdPuntosControlQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetByIdPuntosControlQuery, PuntoControlResponse>
{
    public async Task<Result<PuntoControlResponse>> Handle(
        GetByIdPuntosControlQuery query,
        CancellationToken cancellationToken)
    {
        var puntoControl = await context.PuntosControl
            .Where(pc => pc.Id == query.PuntoControlId)
            .FirstOrDefaultAsync();

        if (puntoControl is null)
        {
            return Result.Failure<PuntoControlResponse>(PuntoControlErrors.NotFound(query.PuntoControlId));
        }

        var tipoPuntoControl = new ItemResponse<int>((int)puntoControl.Tipo, puntoControl.Tipo.ToString());
        var estadoPuntoControl = new ItemResponse<int>((int)puntoControl.Estado, puntoControl.Estado.ToString());

        return new PuntoControlResponse
        (
            puntoControl.Id,
            puntoControl.Nombre,
            puntoControl.Ubicacion,
            tipoPuntoControl,
            estadoPuntoControl,
            puntoControl.Descripcion
        );
    }
}