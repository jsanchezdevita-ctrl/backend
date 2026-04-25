using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using Domain.Enums;
using SharedKernel;

namespace Application.Filtros.GetEstadosPuntoControl;

internal sealed class GetEstadosPuntoControlQueryHandler()
    : IQueryHandler<GetEstadosPuntoControlQuery, PagedResponse<EnumFiltroItemResponse>>
{
    public async Task<Result<PagedResponse<EnumFiltroItemResponse>>> Handle(
        GetEstadosPuntoControlQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = Enum.GetValues<PuntoControlState>()
            .Select(u => new
            {
                Id = (int)u,
                Nombre = u.ToString()
            });

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.Where(x => x.Nombre.Contains(query.SearchTerm));
        }

        var resultQuery = baseQuery.Select(r => new EnumFiltroItemResponse(
            r.Id,
            r.Nombre));

        return await Task.FromResult(resultQuery.ToPagedResponse(query.Page, query.PageSize));
    }
}