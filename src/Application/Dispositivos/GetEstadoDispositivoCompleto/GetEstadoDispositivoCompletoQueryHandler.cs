using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.GetEstadoDispositivoCompleto;

internal sealed class GetEstadoDispositivoCompletoQueryHandler
    : IQueryHandler<GetEstadoDispositivoCompletoQuery, DispositivoCompletoResponse>
{
    private readonly IApplicationDbContext _context;

    public GetEstadoDispositivoCompletoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DispositivoCompletoResponse>> Handle(
        GetEstadoDispositivoCompletoQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Buscar dispositivo con su punto de control (JOIN explícito)
        var dispositivoQuery = await (
            from d in _context.Dispositivos
            join pc in _context.PuntosControl on d.PuntoControlId equals pc.Id into pcJoin
            from pc in pcJoin.DefaultIfEmpty()
            where d.Id == request.DispositivoId
            select new
            {
                d.Id,
                d.Nombre,
                d.DireccionIp,
                d.DispositivoId,
                d.PuntoControlId,
                PuntoControl = pc != null ? new
                {
                    pc.Id,
                    pc.Nombre,
                    pc.Descripcion,
                    pc.Ubicacion
                } : null
            }
        ).FirstOrDefaultAsync(cancellationToken);

        if (dispositivoQuery is null)
        {
            return Result.Failure<DispositivoCompletoResponse>(
                Error.NotFound("Dispositivo.NotFound", $"El dispositivo con ID {request.DispositivoId} no existe"));
        }

        if (dispositivoQuery.PuntoControl is null)
        {
            return Result.Failure<DispositivoCompletoResponse>(
                Error.NotFound("Dispositivo.SinPuntoControl", $"El dispositivo {dispositivoQuery.Nombre} no tiene un punto de control asignado"));
        }

        // 2. Obtener todas las zonas relacionadas a ese punto de control
        var zonasPuntoControl = await (
            from zpc in _context.ZonasPuntosControl
            where zpc.PuntoControlId == dispositivoQuery.PuntoControl.Id
            select zpc.ZonaId
        ).ToListAsync(cancellationToken);

        // 3. Obtener todos los ZonasRoles de esas zonas con sus relaciones (JOIN con Zona y Rol)
        var zonasRoles = await (
            from zr in _context.ZonasRoles
            join z in _context.Zonas on zr.ZonaId equals z.Id
            join r in _context.Roles on zr.RolId equals r.Id
            where zonasPuntoControl.Contains(zr.ZonaId)
            select new ZonaRolInfoResponse
            {
                ZonaRolId = zr.Id,
                ZonaId = z.Id,
                ZonaNombre = z.Nombre,
                RolId = r.Id,
                RolNombre = r.NombreRol,
                CapacidadMaxima = zr.CapacidadMaxima,
                EspacioUtilizado = zr.EspacioUtilizado
            }
        ).ToListAsync(cancellationToken);

        // 4. Armar respuesta completa
        var response = new DispositivoCompletoResponse
        {
            Id = dispositivoQuery.Id,
            Nombre = dispositivoQuery.Nombre,
            DireccionIp = dispositivoQuery.DireccionIp,
            DispositivoId = dispositivoQuery.DispositivoId,
            PuntoControl = new PuntoControlInfoResponse
            {
                PuntoControlId = dispositivoQuery.PuntoControl.Id,
                Nombre = dispositivoQuery.PuntoControl.Nombre,
                Descripcion = dispositivoQuery.PuntoControl.Descripcion,
                Ubicacion = dispositivoQuery.PuntoControl.Ubicacion
            },
            ZonasRoles = zonasRoles
        };

        return response;
    }
}