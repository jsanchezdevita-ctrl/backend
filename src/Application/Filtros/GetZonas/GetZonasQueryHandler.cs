using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Filtros.GetZonas;

internal sealed class GetZonasQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetZonasQuery, PagedResponse<FiltroItemResponse>>
{
    public async Task<Result<PagedResponse<FiltroItemResponse>>> Handle(
        GetZonasQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Zonas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(er => new FiltroItemResponse(
            er.Id,
            er.Nombre));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}