using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Filtros.GetPuntosControl;

internal sealed class GetPuntosControlQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetPuntosControlQuery, PagedResponse<FiltroPuntoControl>>
{
    public async Task<Result<PagedResponse<FiltroPuntoControl>>> Handle(
        GetPuntosControlQuery query,
        CancellationToken cancellationToken)
    {
        var puntosQuery = from pc in context.PuntosControl
                          join d in context.Dispositivos on pc.Id equals d.PuntoControlId into dispositivosGroup
                          from d in dispositivosGroup.DefaultIfEmpty()
                          select new FiltroPuntoControl(
                              pc.Id,
                              pc.Nombre,
                              d != null ? d.Id : Guid.Empty,
                              d != null ? d.Nombre : "Sin dispositivo asignado");

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            puntosQuery = puntosQuery.Where(x =>
                x.Label.Contains(query.SearchTerm) ||
                x.NombreDispositivo.Contains(query.SearchTerm));
        }

        return await puntosQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}