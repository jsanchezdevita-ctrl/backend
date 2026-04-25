using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Filtros.GetEstadosRegistro;

internal sealed class GetEstadosRegistroQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetEstadosRegistroQuery, PagedResponse<FiltroItemResponse>>
{
    public async Task<Result<PagedResponse<FiltroItemResponse>>> Handle(
        GetEstadosRegistroQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.EstadosRegistro.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(er => new FiltroItemResponse(
            er.Id,
            er.Descripcion));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}