using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Filtros.GetTiposUsuario;

internal sealed class GetTiposUsuarioQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTiposUsuarioQuery, PagedResponse<FiltroItemResponse>>
{
    public async Task<Result<PagedResponse<FiltroItemResponse>>> Handle(
        GetTiposUsuarioQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Roles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.NombreRol,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(r => new FiltroItemResponse(
            r.Id,
            r.NombreRol));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}