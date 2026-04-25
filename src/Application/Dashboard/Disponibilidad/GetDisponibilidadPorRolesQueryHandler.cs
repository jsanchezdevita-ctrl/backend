using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;

internal sealed class GetDisponibilidadPorRolesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDisponibilidadPorRolesQuery, DisponibilidadPorRolesResponse>
{
    public async Task<Result<DisponibilidadPorRolesResponse>> Handle(
        GetDisponibilidadPorRolesQuery query,
        CancellationToken cancellationToken)
    {
        var zona = await context.Zonas
            .Where(z => z.Id == query.zonaId && !z.Deleted)
            .FirstOrDefaultAsync();

        if (zona is null)
        {
            return Result.Failure<DisponibilidadPorRolesResponse>(ZonaErrors.NotFound(query.zonaId));
        }

        var roles = await context.Roles
            .Where(r => !r.Deleted)
            .Select(r => new { r.Id, r.NombreRol, r.EsAdmin })
            .ToListAsync(cancellationToken);

        var disponibilidadRoles = new List<DisponibilidadRolSummary>();

        foreach (var rol in roles)
        {
            var zonaRolesDelRol = await context.ZonasRoles
                .Where(zr => zr.RolId == rol.Id && 
                             zr.ZonaId == zona.Id &&
                            !zr.Deleted)
                .ToListAsync(cancellationToken);

            if (zonaRolesDelRol.Count <= 0)
                continue;

            var totalCapacidad = zonaRolesDelRol.Sum(zr => zr.CapacidadMaxima);
            var espacioUtilizado = zonaRolesDelRol.Sum(zr => zr.EspacioUtilizado);
            var espaciosLibres = totalCapacidad - espacioUtilizado;

            var porcentajeOcupado = totalCapacidad > 0
                ? Math.Round((espacioUtilizado / (double)totalCapacidad) * 100, 1)
                : 0;

            var estado = DeterminarEstado(porcentajeOcupado, espaciosLibres);

            disponibilidadRoles.Add(new DisponibilidadRolSummary(
                rol.Id,
                rol.NombreRol,
                rol.EsAdmin,
                totalCapacidad,
                espacioUtilizado,
                espaciosLibres,
                porcentajeOcupado,
                estado));
        }

        disponibilidadRoles = disponibilidadRoles
            .OrderBy(dr => dr.RolNombre)
            .ToList();

        return new DisponibilidadPorRolesResponse(disponibilidadRoles);
    }

    private static string DeterminarEstado(double porcentajeOcupado, int espaciosLibres)
    {
        if (porcentajeOcupado >= 90 || espaciosLibres <= 0)
            return "LLENO";

        if (porcentajeOcupado >= 80)
            return "LIMITE";

        return "DISPONIBLE";
    }
}