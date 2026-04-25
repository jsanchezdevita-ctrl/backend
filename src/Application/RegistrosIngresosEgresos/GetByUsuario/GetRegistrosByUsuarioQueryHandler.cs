using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.RegistrosIngresosEgresos;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.RegistrosIngresosEgresos.GetByUsuario;

internal sealed class GetRegistrosByUsuarioQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetRegistrosByUsuarioQuery, List<RegistroIngresoEgresoUsuarioResponse>>
{
    public async Task<Result<List<RegistroIngresoEgresoUsuarioResponse>>> Handle(
        GetRegistrosByUsuarioQuery query,
        CancellationToken cancellationToken)
    {
        // Validar que el usuario existe
        var usuarioExiste = await context.Usuarios
            .AnyAsync(u => u.Id == query.UsuarioId && !u.Deleted, cancellationToken);

        if (!usuarioExiste)
        {
            return Result.Failure<List<RegistroIngresoEgresoUsuarioResponse>>(
                RegistroIngresoEgresoErrors.UsuarioNotFound(query.UsuarioId));
        }

        // Calcular fecha límite (últimos 30 días)
        var fechaLimite = DateTime.UtcNow.Date.AddDays(-30);

        // Obtener registros del usuario de los últimos 30 días
        var registros = await context.RegistrosIngresosEgresos
            .AsNoTracking()
            .Where(r => r.UsuarioId == query.UsuarioId &&
                       r.Fecha >= fechaLimite &&
                       !r.Deleted)
            .OrderByDescending(r => r.Fecha) // Más recientes primero
            .Select(r => new
            {
                Registro = r,
                // Determinar si es ingreso o egreso
                EsIngreso = r.PuntoEntradaId != null,
                PuntoControl = r.PuntoEntradaId != null
                    ? context.PuntosControl
                        .Where(pc => pc.Id == r.PuntoEntradaId && !pc.Deleted)
                        .FirstOrDefault()
                    : context.PuntosControl
                        .Where(pc => pc.Id == r.PuntoSalidaId && !pc.Deleted)
                        .FirstOrDefault(),
                EstadoRegistro = context.EstadosRegistro
                    .Where(er => er.Id == r.EstadoRegistroId && !er.Deleted)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        // Mapear a la respuesta
        var response = registros
            .Where(x => x.PuntoControl != null && x.EstadoRegistro != null)
            .Select(x => new RegistroIngresoEgresoUsuarioResponse(
                Id: x.Registro.Id,
                FechaHora: x.Registro.Fecha,
                HaceTiempo: TimeAgoHelper.GetTimeAgoText(x.Registro.Fecha),
                TipoRegistro: new TipoRegistroInfo(
                    x.EsIngreso ? "Ingreso" : "Egreso"),
                PuntoAcceso: new PuntoAccesoInfo(
                    x.PuntoControl.Id,
                    x.PuntoControl.Nombre),
                Ubicacion: x.PuntoControl.Ubicacion,
                Estado: new EstadoInfo(
                    x.EstadoRegistro.Id,
                    x.EstadoRegistro.Descripcion)))
            .ToList();

        return response;
    }
}