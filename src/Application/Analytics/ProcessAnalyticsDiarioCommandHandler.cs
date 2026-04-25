using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Analytics.AccesosDiaSemana;
using Domain.Analytics.AccesosIncidencias;
using Domain.Analytics.AccesosPorHora;
using Domain.Analytics.AccesosTipoUsuario;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analytics;

internal sealed class ProcessAnalyticsCommandHandler(
    IApplicationDbContext contextPrincipal,
    IAnalyticsDbContext contextAnalytics)
    : ICommandHandler<ProcessAnalyticsCommand>
{
    public async Task<Result> Handle(
        ProcessAnalyticsCommand command,
        CancellationToken cancellationToken)
    {
        await ProcessDateRange(cancellationToken);

        return Result.Success();
    }

    private async Task ProcessDateRange(CancellationToken cancellationToken)
    {
        var data = await GetRegistrosIngresosEgresosByDateRange(cancellationToken);

        await ProcesarAccesosTipoUsuario(data,cancellationToken);
        await ProcesarAccesosPorHora(data, cancellationToken);
        await ProcesarAccesosDiaSemana(data, cancellationToken);
        await ProcesarAccesosIncidencias(data, cancellationToken);
    }

    private async Task<List<RegistroCompleto>> GetRegistrosIngresosEgresosByDateRange(
        CancellationToken cancellationToken)
    {
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var startDate = endDate.AddDays(-1);

        var fechaDesde = DateTime.SpecifyKind(startDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        var fechaHasta = DateTime.SpecifyKind(endDate.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

        var query =
            from reg in contextPrincipal.RegistrosIngresosEgresos
            join u in contextPrincipal.Usuarios on reg.UsuarioId equals u.Id
            join ro in contextPrincipal.Roles on reg.RolId equals ro.Id
            join e in contextPrincipal.EstadosRegistro on reg.EstadoRegistroId equals e.Id
            join pe in contextPrincipal.PuntosControl on reg.PuntoEntradaId equals pe.Id into pe
            from peAux in pe.DefaultIfEmpty()
            join ps in contextPrincipal.PuntosControl on reg.PuntoSalidaId equals ps.Id into ps
            from psAux in ps.DefaultIfEmpty()
            where reg.Fecha >= fechaDesde &&
                  reg.Fecha < fechaHasta
            select new RegistroCompleto(
                reg.Id,
                reg.Fecha,
                reg.UsuarioId,
                ro.NombreRol,
                reg.PuntoEntradaId,
                peAux != null ? peAux.Nombre : null,
                reg.PuntoSalidaId,
                psAux != null ? psAux.Nombre : null,
                reg.EstadoRegistroId,
                e.Descripcion
            );

        return await query.ToListAsync(cancellationToken);
    }

    private async Task ProcesarAccesosTipoUsuario(
        List<RegistroCompleto> datos,
        CancellationToken cancellationToken)
    {
        var accesosPorDiaYTipo = datos
            .GroupBy(x => new {
                Fecha = x.Fecha.Date,
                x.TipoUsuario
            })
            .Select(g => new {
                Fecha = g.Key.Fecha,
                g.Key.TipoUsuario,
                Total = g.Count()
            })
            .ToList();

        foreach (var acceso in accesosPorDiaYTipo)
        {
            var fechaParaBD = DateTime.SpecifyKind(acceso.Fecha, DateTimeKind.Unspecified);

            var existing = await contextAnalytics.AccesosTipoUsuario
                .FirstOrDefaultAsync(a => a.Fecha == fechaParaBD &&
                                         a.TipoUsuario == acceso.TipoUsuario,
                                   cancellationToken);

            if (existing != null)
            {
                existing.Cantidad = acceso.Total;
            }
            else
            {
                contextAnalytics.AccesosTipoUsuario.Add(new AccesoTipoUsuario
                {
                    Id = Guid.NewGuid(),
                    Cantidad = acceso.Total,
                    TipoUsuario = acceso.TipoUsuario,
                    Fecha = fechaParaBD
                });
            }
        }

        await contextAnalytics.SaveChangesAsync(cancellationToken);
    }
    
    private async Task ProcesarAccesosPorHora(
    List<RegistroCompleto> datos,
    CancellationToken cancellationToken)
    {
        var accesosPorHora = datos
            .GroupBy(x => new {
                Fecha = x.Fecha.Date,
                Hora = x.Fecha.Hour
            })
            .Select(g => new {
                g.Key.Fecha,
                g.Key.Hora,
                Total = g.Count()
            })
            .ToList();

        foreach (var acceso in accesosPorHora)
        {
            var fechaParaBD = DateTime.SpecifyKind(acceso.Fecha, DateTimeKind.Unspecified);

            var existing = await contextAnalytics.AccesosPorHora
                .FirstOrDefaultAsync(a => a.Fecha == fechaParaBD &&
                                         a.Hora == acceso.Hora,
                                   cancellationToken);

            if (existing != null)
            {
                existing.Cantidad = acceso.Total;
            }
            else
            {
                contextAnalytics.AccesosPorHora.Add(new AccesoPorHora
                {
                    Id = Guid.NewGuid(),
                    Cantidad = acceso.Total,
                    Hora = acceso.Hora,
                    Fecha = fechaParaBD
                });
            }
        }

        await contextAnalytics.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcesarAccesosDiaSemana(
    List<RegistroCompleto> datos,
    CancellationToken cancellationToken)
    {
        var accesosPorDiaSemana = datos
            .GroupBy(x => new {
                Fecha = x.Fecha.Date,
                DiaInfo = x.Fecha.ToDiaSemanaEspanol()
            })
            .Select(g => new {
                g.Key.Fecha,
                DiaCompleto = g.Key.DiaInfo.Completo,
                DiaCorto = g.Key.DiaInfo.Corto,
                Total = g.Count()
            })
            .ToList();

        foreach (var acceso in accesosPorDiaSemana)
        {
            var fechaParaBD = DateTime.SpecifyKind(acceso.Fecha, DateTimeKind.Unspecified);

            var existing = await contextAnalytics.AccesosDiaSemana
                .FirstOrDefaultAsync(a => a.Fecha == fechaParaBD &&
                                         a.DiaSemanaCompleto == acceso.DiaCompleto,
                                   cancellationToken);

            if (existing != null)
            {
                existing.Cantidad = acceso.Total;
            }
            else
            {
                contextAnalytics.AccesosDiaSemana.Add(new AccesoDiaSemana
                {
                    Id = Guid.NewGuid(),
                    Cantidad = acceso.Total,
                    DiaSemanaCompleto = acceso.DiaCompleto,
                    DiaSemanaCorto = acceso.DiaCorto,
                    Fecha = fechaParaBD
                });
            }
        }

        await contextAnalytics.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcesarAccesosIncidencias(
        List<RegistroCompleto> datos,
        CancellationToken cancellationToken)
    {
        var datosConPuntoControl = datos
            .Select(x => new
            {
                Fecha = x.Fecha.Date,
                PuntoControlId = x.PuntoEntradaId ?? x.PuntoSalidaId,
                NombrePuntoControl = x.NombrePuntoEntrada ?? x.NombrePuntoSalida,
                x.DescripcionEstadoRegistro
            })
            .Where(x => x.PuntoControlId.HasValue && !string.IsNullOrEmpty(x.NombrePuntoControl))
            .ToList();

        var accesosPorIncidencia = datosConPuntoControl
            .GroupBy(x => new {
                Fecha = x.Fecha,
                PuntoControlId = x.PuntoControlId.Value,
                NombrePuntoControl = x.NombrePuntoControl,
                Incidencia = x.DescripcionEstadoRegistro
            })
            .Select(g => new {
                Fecha = g.Key.Fecha,
                PuntoControlId = g.Key.PuntoControlId,
                NombrePuntoControl = g.Key.NombrePuntoControl,
                Incidencia = g.Key.Incidencia,
                Total = g.Count()
            })
            .ToList();

        foreach (var acceso in accesosPorIncidencia)
        {
            var fechaParaBD = DateTime.SpecifyKind(acceso.Fecha, DateTimeKind.Unspecified);

            var existing = await contextAnalytics.AccesosIncidencias
                .FirstOrDefaultAsync(a => a.Fecha == fechaParaBD &&
                                         a.PuntoControlId == acceso.PuntoControlId &&
                                         a.Incidencia == acceso.Incidencia,
                                   cancellationToken);

            if (existing != null)
            {
                existing.Cantidad = acceso.Total;
                existing.NombrePuntoControl = acceso.NombrePuntoControl;
            }
            else
            {
                contextAnalytics.AccesosIncidencias.Add(new AccesoIncidencia
                {
                    Id = Guid.NewGuid(),
                    Cantidad = acceso.Total,
                    PuntoControlId = acceso.PuntoControlId,
                    NombrePuntoControl = acceso.NombrePuntoControl,
                    Incidencia = acceso.Incidencia,
                    Fecha = fechaParaBD
                });
            }
        }

        await contextAnalytics.SaveChangesAsync(cancellationToken);
    }

    private sealed record RegistroCompleto(
        Guid Id,
        DateTime Fecha,
        Guid UsuarioId,
        string TipoUsuario,
        Guid? PuntoEntradaId,
        string? NombrePuntoEntrada,
        Guid? PuntoSalidaId,
        string? NombrePuntoSalida,
        Guid EstadoRegistroId,
        string DescripcionEstadoRegistro);
}