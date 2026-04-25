using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Zonas.GetAll;

internal sealed class GetAllZonasQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllZonasQuery, PagedResponse<ZonaResponse>>
{
    public async Task<Result<PagedResponse<ZonaResponse>>> Handle(
        GetAllZonasQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Zonas.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                z => z.Nombre,
                z => z.Descripcion);
        }

        var resultQuery = baseQuery.Select(z => new ZonaResponse(
            z.Id,
            z.Nombre,
            z.Descripcion,

            context.ZonasRoles
                .Where(zr => zr.ZonaId == z.Id)
                .Join(context.Roles,
                    zr => zr.RolId,
                    r => r.Id,
                    (zr, r) => new ZonaRolInfo(
                        zr.Id,
                        r.Id,
                        r.NombreRol,
                        zr.CapacidadMaxima,
                        zr.EspacioUtilizado,
                        zr.CapacidadMaxima - zr.EspacioUtilizado))
                .ToList(),

            context.ZonasPuntosControl
                .Where(zpc => zpc.ZonaId == z.Id)
                .Join(context.PuntosControl,
                    zpc => zpc.PuntoControlId,
                    pc => pc.Id,
                    (zpc, pc) => new ZonaPuntoControlInfo(
                        zpc.Id,
                        pc.Id,
                        pc.Nombre,
                        pc.Ubicacion))
                .ToList()));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}