using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Parametros.GetAll;

internal sealed class GetAllParametrosQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllParametrosQuery, PagedResponse<ParametroResponse>>
{
    public async Task<Result<PagedResponse<ParametroResponse>>> Handle(
        GetAllParametrosQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Parametros.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Llave,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(p => new ParametroResponse(
            p.Id,
            p.Llave,
            p.Valor,
            p.Descripcion,
            p.FechaActualizacion,
            p.ActualizadoPor,
            p.Version));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}