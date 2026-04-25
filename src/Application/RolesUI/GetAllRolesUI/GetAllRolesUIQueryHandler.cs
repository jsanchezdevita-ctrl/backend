using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.RolesUI.GetAllRolesUI;

internal sealed class GetAllRolesUIQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllRolesUIQuery, PagedResponse<RolUIResponse>>
{
    public async Task<Result<PagedResponse<RolUIResponse>>> Handle(
        GetAllRolesUIQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.RolesUI
            .Join(context.Roles,
                rui => rui.RolId,
                r => r.Id,
                (rui, r) => new { RolUI = rui, Rol = r });

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Rol.NombreRol,
                x => x.Rol.Descripcion,
                x => x.RolUI.TextColor,
                x => x.RolUI.BackgroundColor);
        }

        var resultQuery = baseQuery
            .OrderBy(x => x.Rol.NombreRol)
            .Select(x => new RolUIResponse(
                x.Rol.Id,
                x.Rol.NombreRol,
                x.RolUI.TextColor,
                x.RolUI.BackgroundColor));

        return await resultQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}