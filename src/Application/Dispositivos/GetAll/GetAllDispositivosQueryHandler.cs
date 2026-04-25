using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Dispositivos.GetAll;

internal sealed class GetAllDispositivosQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllDispositivosQuery, PagedResponse<DispositivoResponse>>
{
    public async Task<Result<PagedResponse<DispositivoResponse>>> Handle(
        GetAllDispositivosQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Dispositivos.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.DispositivoId,
                x => x.Nombre,
                x => x.DireccionIp);
        }

        var resultQuery = baseQuery.Select(d => new DispositivoResponse(
            d.Id,
            d.DispositivoId,
            d.Nombre,
            d.DireccionIp,
            d.UltimaConexion,
            context.PuntosControl
                .Where(pc => pc.Id == d.PuntoControlId)
                .Select(pc => new PuntoControlInfo(
                    pc.Id,
                    pc.Nombre,
                    pc.Ubicacion,
                    pc.Tipo.ToString()))
                .First(),
            context.DispositivoConfiguraciones
                .Where(dc => dc.DispositivoId == d.Id)
                .Select(dc => new DispositivoConfiguracionResponse(
                    dc.FrecuenciaSincronizacionSegundos,
                    dc.PotenciaTransmision.ToString(),
                    dc.CanalComunicacion))
                .FirstOrDefault()));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}