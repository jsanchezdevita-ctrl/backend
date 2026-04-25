using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Parametros.GetAllSchemas;

internal sealed class GetAllParametroSchemasQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllParametroSchemasQuery, PagedResponse<ParametroSchemaResponse>>
{
    public async Task<Result<PagedResponse<ParametroSchemaResponse>>> Handle(
        GetAllParametroSchemasQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.ParametroSchemas.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Llave);
        }

        var resultQuery = baseQuery.Select(ps => new ParametroSchemaResponse(
            ps.Id,
            ps.Llave,
            ps.Schema,
            ps.FechaCreacion,
            ps.FechaActualizacion));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}