using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.PuntosControl.GetAll;

internal sealed class GetAllPuntosControlQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllPuntosControlQuery, PagedResponse<PuntoControlResponse>>
{
    public async Task<Result<PagedResponse<PuntoControlResponse>>> Handle(
        GetAllPuntosControlQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.PuntosControl.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Nombre,
                x => x.Ubicacion,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(p => new PuntoControlResponse(
            p.Id,
            p.Nombre,
            p.Ubicacion,
            p.Tipo.ToString(),
            p.Estado.ToString(),
            p.Descripcion));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}