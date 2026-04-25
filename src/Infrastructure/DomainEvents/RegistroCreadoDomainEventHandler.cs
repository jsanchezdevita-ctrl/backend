using Application.Abstractions.Data;
using Application.Abstractions.Realtime;
using Application.Dashboard.Disponibilidad.GetDisponibilidadPorRoles;
using Application.RegistrosIngresosEgresos.Create;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.DomainEvents;

internal sealed class RegistroCreadoDomainEventHandler(
    IDisponibilidadNotifier disponibilidadNotifier,
    IApplicationDbContext dbContext) : IDomainEventHandler<RegistroCreadoDomainEvent>
{
    public async Task Handle(RegistroCreadoDomainEvent notification, CancellationToken cancellationToken)
    {
        var roles = await dbContext.Roles
            .Where(r => !r.Deleted)
            .Select(r => new { r.Id, r.NombreRol, r.EsAdmin })
            .ToListAsync(cancellationToken);

        var disponibilidadRoles = new List<DisponibilidadRolSummary>();

        foreach (var rol in roles)
        {
            var zonaRolesDelRol = await dbContext.ZonasRoles
                .Where(zr => zr.RolId == rol.Id && !zr.Deleted)
                .ToListAsync(cancellationToken);

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

        var response = new DisponibilidadPorRolesResponse(disponibilidadRoles);

        await disponibilidadNotifier.NotifyDisponibilidadActualizada(response);
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