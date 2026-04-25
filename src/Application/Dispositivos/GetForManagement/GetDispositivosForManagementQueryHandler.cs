using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Dispositivos.GetForManagement;

internal sealed class GetDispositivosForManagementQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDispositivosForManagementQuery, PagedResponse<DispositivoResponse>>
{
    public async Task<Result<PagedResponse<DispositivoResponse>>> Handle(
        GetDispositivosForManagementQuery query,
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

        var dispositivosQuery = baseQuery.Select(d => new DispositivoResponse(
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

        return await dispositivosQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}