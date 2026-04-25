using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.Filtros.GetRoles;

internal sealed class GetRolesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetRolesQuery, PagedResponse<FiltroItemResponse>>
{
    public async Task<Result<PagedResponse<FiltroItemResponse>>> Handle(
        GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.Roles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Descripcion);
        }

        var resultQuery = baseQuery.Select(er => new FiltroItemResponse(
            er.Id,
            er.NombreRol));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}