using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Paginations;
using SharedKernel;

namespace Application.RegistrosIngresosEgresos.GetForMonitoring;

internal sealed class GetRegistrosForMonitoringQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetRegistrosForMonitoringQuery, PagedResponse<RegistroIngresoEgresoMonitoringResponse>>
{
    public async Task<Result<PagedResponse<RegistroIngresoEgresoMonitoringResponse>>> Handle(
        GetRegistrosForMonitoringQuery query,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.RegistrosIngresosEgresos
            .Join(context.Usuarios,
                r => r.UsuarioId,
                u => u.Id,
                (r, u) => new { Registro = r, Usuario = u })
            .Join(context.Roles,
                x => x.Registro.RolId,
                r => r.Id,
                (x, rol) => new { x.Registro, x.Usuario, Rol = rol })
            .Join(context.PuntosControl,
                x => x.Registro.PuntoEntradaId ?? x.Registro.PuntoSalidaId ?? Guid.Empty,
                pc => pc.Id,
                (x, pc) => new { x.Registro, x.Usuario, x.Rol, PuntoControl = pc })
            .Join(context.EstadosRegistro,
                x => x.Registro.EstadoRegistroId,
                er => er.Id,
                (x, er) => new
                {
                    x.Registro,
                    x.Usuario,
                    x.Rol,
                    x.PuntoControl,
                    EstadoRegistro = er
                });

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.SearchByTerm(query.SearchTerm,
                x => x.Usuario.Nombre,
                x => x.Usuario.Apellido,
                x => x.Usuario.NumeroDocumento,
                x => x.PuntoControl.Nombre,
                x => x.PuntoControl.Ubicacion);
        }

        if (query.EstadoRegistroId.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Registro.EstadoRegistroId == query.EstadoRegistroId.Value);
        }

        if (query.TipoUsuarioId.HasValue) 
        {
            baseQuery = baseQuery.Where(x => x.Rol.Id == query.TipoUsuarioId.Value);
        }

        if (query.PuntoAccesoId.HasValue)
        {
            baseQuery = baseQuery.Where(x =>
                x.Registro.PuntoEntradaId == query.PuntoAccesoId.Value ||
                x.Registro.PuntoSalidaId == query.PuntoAccesoId.Value);
        }

        if (query.FechaDesde.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Registro.Fecha >= query.FechaDesde.Value);
        }

        if (query.FechaHasta.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Registro.Fecha <= query.FechaHasta.Value);
        }

        var registrosQuery = baseQuery
            .OrderByDescending(x => x.Registro.Fecha)
            .Select(x => new RegistroIngresoEgresoMonitoringResponse(
                x.Registro.Id,
                x.Registro.Fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                new UsuarioInfo(
                    x.Usuario.Id,
                    $"{x.Usuario.Nombre} {x.Usuario.Apellido}",
                    x.Usuario.NumeroDocumento),
                new TipoInfo(
                    x.Rol.Id,
                    x.Rol.NombreRol),
                new PuntoAccesoInfo(
                    x.PuntoControl.Id,
                    x.PuntoControl.Nombre),
                x.PuntoControl.Ubicacion,
                new EstadoInfo(
                    x.EstadoRegistro.Id,
                    x.EstadoRegistro.Descripcion)));

        return await registrosQuery.ToPagedResponseAsync(
            query.Page, query.PageSize, cancellationToken);
    }
}