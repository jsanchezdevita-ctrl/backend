using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using Application.Filtros.GetDispositivos;
using SharedKernel;

namespace Application.Filtros.GetRoles;

internal sealed class GetDispositivosQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDispositivosQuery, PagedResponse<FiltroItemResponse>>
{
    public async Task<Result<PagedResponse<FiltroItemResponse>>> Handle(
        GetDispositivosQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Dispositivos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Nombre);
        }

        var resultQuery = baseQuery.Select(er => new FiltroItemResponse(
            er.Id,
            er.Nombre));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}
