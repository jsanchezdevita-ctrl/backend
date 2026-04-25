using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.PuntosControl.GetForManagement;

internal sealed class GetPuntosControlForManagementQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetPuntosControlForManagementQuery, PuntosControlForManagementResponse>
{
    public async Task<Result<PuntosControlForManagementResponse>> Handle(
        GetPuntosControlForManagementQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.PuntosControl.OrderByDescending(o => o.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Nombre,
                x => x.Ubicacion,
                x => x.Descripcion);
        }

        var puntosControlQuery = baseQuery.Select(x => new PuntoControlConRolResponse(
            x.Id,
            x.Nombre,
            x.Ubicacion,
            x.Tipo.ToString(),
            x.Estado.ToString(),
            x.Descripcion));

        var puntosControlPaginados = await puntosControlQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);

        var roles = await context.Roles
            .Select(r => new RolMetadata(r.Id, r.NombreRol, r.Descripcion))
            .ToListAsync(cancellationToken);

        var tipos = Enum.GetValues(typeof(PuntoControlType))
            .Cast<PuntoControlType>()
            .Select(t => new TipoMetadata(
                (int)t,
                t.ToString(),
                GetTipoDescription(t)))
            .ToList();

        var estados = Enum.GetValues(typeof(PuntoControlState))
            .Cast<PuntoControlState>()
            .Select(e => new EstadoMetadata(
                (int)e,
                e.ToString(),
                GetEstadoDescription(e)))
            .ToList();

        return new PuntosControlForManagementResponse(
            puntosControlPaginados, roles, tipos, estados);
    }

    private static string GetTipoDescription(PuntoControlType tipo)
    {
        return tipo switch
        {
            PuntoControlType.Entrada => "Punto de control para registrar entrada",
            PuntoControlType.Salida => "Punto de control para registrar salida",
            _ => "Tipo desconocido"
        };
    }

    private static string GetEstadoDescription(PuntoControlState estado)
    {
        return estado switch
        {
            PuntoControlState.Activo => "Punto de control activo y operativo",
            PuntoControlState.Inactivo => "Punto de control inactivo",
            _ => "Estado desconocido"
        };
    }
}