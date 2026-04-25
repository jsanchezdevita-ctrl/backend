using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using Domain.Enums;
using SharedKernel;

namespace Application.Filtros.GetEstadosUsuario;

internal sealed class GetEstadosUsuarioQueryHandler()
    : IQueryHandler<GetEstadosUsuarioQuery, PagedResponse<EnumFiltroItemResponse>>
{
    public async Task<Result<PagedResponse<EnumFiltroItemResponse>>> Handle(
        GetEstadosUsuarioQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = Enum.GetValues<UsuarioState>()
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