using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Application.Dispositivos.GetEstadoDispositivoCompleto;
using Application.Zonas.GetZonasEstadoMobile;
using Application.Zonas.GetZonasEstadoWeb;
using Domain.RegistrosIngresosEgresos;
using Domain.Usuarios;
using Domain.ZonasRoles;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.RegistrosIngresosEgresos.Create;

internal sealed class CreateRegistroIngresoEgresoCommandHandler(
    IApplicationDbContext context,
    IZonaNotifier zonaNotifier,
    IZonaNotifierWeb zonaNotifierWeb,
    IMonitoreoNotifier monitoreoNotifier,
    IDispositivoEstadoNotifier dispositivoNotifier,
    IQueryHandler<GetZonasEstadoMobileQuery, List<ZonaEstadoMobileResponse>> queryHandler,
    IQueryHandler<GetZonasEstadoWebQuery, List<ZonaEstadoWebResponse>> webQueryHandler,
    IQueryHandler<GetEstadoDispositivoCompletoQuery, DispositivoCompletoResponse> dispositivoQueryHandler)
    : ICommandHandler<CreateRegistroIngresoEgresoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateRegistroIngresoEgresoCommand command,
        CancellationToken cancellationToken)
    {
        // ===== VALIDACIONES FATAL =====
        bool usuarioExiste = await context.Usuarios
            .AnyAsync(u => u.Id == command.UsuarioId, cancellationToken);

        if (!usuarioExiste)
        {
            return Result.Failure<Guid>(UsuarioErrors.NotFound(command.UsuarioId));
        }

        if (!command.PuntoEntradaId.HasValue && !command.PuntoSalidaId.HasValue)
        {
            return Result.Failure<Guid>(RegistroIngresoEgresoErrors.PuntoControlRequired);
        }

        if (command.PuntoEntradaId.HasValue && command.PuntoSalidaId.HasValue)
        {
            return Result.Failure<Guid>(RegistroIngresoEgresoErrors.AmbosPuntosNoPermitidos);
        }

        Guid puntoControlId = command.PuntoEntradaId ?? command.PuntoSalidaId!.Value;
        var puntoControl = await context.PuntosControl
            .FirstOrDefaultAsync(p => p.Id == puntoControlId, cancellationToken);

        if (puntoControl is null)
        {
            string tipo = command.PuntoEntradaId.HasValue ? "entrada" : "salida";
            return Result.Failure<Guid>(RegistroIngresoEgresoErrors.PuntoControlNotFound(puntoControlId, tipo));
        }

        bool esEntrada = command.PuntoEntradaId.HasValue;

        var estadoRegistro = await context.EstadosRegistro
            .FirstOrDefaultAsync(er => er.Id == command.EstadoRegistroId, cancellationToken);

        if (estadoRegistro is null)
        {
            return Result.Failure<Guid>(RegistroIngresoEgresoErrors.EstadoRegistroNotFound(command.EstadoRegistroId));
        }

        // ===== VALIDACIONES NO FATAL =====

        // Variables para el registro
        string? observacion = null;
        ZonaRol? zonaRol = null;
        Guid? rolId = null;
        Guid? zonaId = null;
        bool esDenegado = false;

        // 1. Validar ZonaRol
        var zonaRolConRol = await context.ZonasRoles
            .Where(zr => zr.Id == command.ZonaRolId)
            .Join(context.Roles,
                zr => zr.RolId,
                r => r.Id,
                (zr, r) => new { ZonaRol = zr, RolId = r.Id })
            .FirstOrDefaultAsync(cancellationToken);

        if (zonaRolConRol == null)
        {
            observacion = $"ZonaRol no encontrado: {command.ZonaRolId}";
            esDenegado = true;
        }
        else
        {
            zonaRol = zonaRolConRol.ZonaRol;
            rolId = zonaRolConRol.RolId;
            zonaId = zonaRol.ZonaId;
        }

        // ===== NUEVA VALIDACIÓN NO FATAL: SECUENCIA ENTRADA/SALIDA =====
        // Solo aplicar si aún no ha sido denegado y el estado es Autorizado
        if (!esDenegado && estadoRegistro.Descripcion == "Autorizado" && estadoRegistro.AfectaEspacio)
        {
            var (esValido, mensajeError) = await ValidarSecuenciaEntradaSalida(
                command.UsuarioId,
                esEntrada,
                cancellationToken);

            if (!esValido)
            {
                observacion = mensajeError;
                esDenegado = true;
            }
        }
        // ===== FIN VALIDACIÓN SECUENCIA =====

        // 2. Si ya es denegado, no seguimos validando
        if (!esDenegado)
        {
            // Validar que usuario tenga el rol
            bool usuarioTieneRol = await context.UsuariosRoles
                .AnyAsync(ur => ur.UsuarioId == command.UsuarioId &&
                               ur.RolId == rolId, cancellationToken);

            if (!usuarioTieneRol)
            {
                observacion = $"Usuario {command.UsuarioId} no tiene el rol {rolId}";
                esDenegado = true;
            }
        }

        // 3. Si ya es denegado, no seguimos validando
        if (!esDenegado)
        {
            // Validar que punto control pertenezca a la zona
            bool puntoControlEnZona = await context.ZonasPuntosControl
                .AnyAsync(zpc => zpc.ZonaId == zonaRol!.ZonaId &&
                                zpc.PuntoControlId == puntoControl.Id, cancellationToken);

            if (!puntoControlEnZona)
            {
                observacion = $"Punto control {puntoControl.Id} no pertenece a zona {zonaRol!.ZonaId}";
                esDenegado = true;
            }
        }

        // 4. Si el estado es Autorizado, validar capacidad
        if (!esDenegado && estadoRegistro.Descripcion == "Autorizado" && estadoRegistro.AfectaEspacio)
        {
            if (esEntrada)
            {
                if (zonaRol!.EspacioUtilizado >= zonaRol.CapacidadMaxima)
                {
                    observacion = $"Capacidad máxima alcanzada: {zonaRol.CapacidadMaxima}";
                    esDenegado = true;
                }
            }
            else
            {
                if (zonaRol!.EspacioUtilizado <= 0)
                {
                    observacion = "No hay espacio utilizado para registrar salida";
                    esDenegado = true;
                }
            }
        }

        // ===== DETERMINAR ESTADO FINAL =====
        Guid estadoFinalId;

        if (esDenegado)
        {
            // Buscar estado Denegado
            var estadoDenegado = await context.EstadosRegistro
                .FirstOrDefaultAsync(e => e.Descripcion == "Denegado" && !e.AfectaEspacio, cancellationToken);

            if (estadoDenegado is null)
            {
                return Result.Failure<Guid>(Error.NotFound("Estado.Denegado", "Estado 'Denegado' no configurado"));
            }
            estadoFinalId = estadoDenegado.Id;
        }
        else
        {
            estadoFinalId = estadoRegistro.Id; // Usa el que vino en el command (Autorizado)
        }

        // ===== CREAR REGISTRO =====
        var registro = new RegistroIngresoEgreso
        {
            Id = Guid.NewGuid(),
            Fecha = DateTime.UtcNow,
            UsuarioId = command.UsuarioId,
            PuntoEntradaId = command.PuntoEntradaId,
            PuntoSalidaId = command.PuntoSalidaId,
            EstadoRegistroId = estadoFinalId,
            ZonaId = zonaId,
            RolId = rolId,
            Observacion = observacion
        };

        context.RegistrosIngresosEgresos.Add(registro);
        registro.Raise(new RegistroCreadoDomainEvent(registro.Id));

        // ===== ACTUALIZAR ESPACIO (solo si es Autorizado) =====
        if (!esDenegado && estadoRegistro.AfectaEspacio)
        {
            if (esEntrada)
                zonaRol!.EspacioUtilizado += 1;
            else
                zonaRol!.EspacioUtilizado -= 1;
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);

            // ===== NOTIFICAR (solo si hubo cambio de espacio) =====
            if (!esDenegado && estadoRegistro.AfectaEspacio)
            {
                await NotificarCambioZonas(rolId!.Value, command.UsuarioId, cancellationToken);
                await NotificarWeb(zonaRol!.ZonaId, cancellationToken);
                await NotificarDispositivosRelacionados(puntoControlId, cancellationToken);
                await monitoreoNotifier.NotificarActualizacion(cancellationToken);
            }

            return registro.Id;
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure<Guid>(RegistroIngresoEgresoErrors.ConcurrencyError);
        }
    }

    private async Task NotificarCambioZonas(Guid rolId, Guid usuarioId, CancellationToken cancellationToken)
    {
        var query = new GetZonasEstadoMobileQuery(usuarioId, rolId);
        var result = await queryHandler.Handle(query, cancellationToken);

        if (result.IsSuccess)
        {
            await zonaNotifier.NotificarCambioZonas(rolId, usuarioId, result.Value, cancellationToken);
        }
    }

    private async Task NotificarWeb(Guid zonaIdModificada, CancellationToken cancellationToken)
    {
        var query = new GetZonasEstadoWebQuery();
        var result = await webQueryHandler.Handle(query, cancellationToken);

        if (result.IsSuccess)
        {
            await zonaNotifierWeb.NotificarCambioZonas(zonaIdModificada, result.Value, cancellationToken);
        }
    }

    private async Task NotificarDispositivosRelacionados(Guid puntoControlId, CancellationToken cancellationToken)
    {
        // Buscar todos los dispositivos que tienen este punto de control
        var dispositivos = await context.Dispositivos
            .Where(d => d.PuntoControlId == puntoControlId)
            .Select(d => d.Id)
            .ToListAsync(cancellationToken);

        foreach (var dispositivoId in dispositivos)
        {
            // Obtener datos completos del dispositivo
            var query = new GetEstadoDispositivoCompletoQuery(dispositivoId);
            var result = await dispositivoQueryHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
            {
                // Notificar a los clientes que están monitoreando este dispositivo
                await dispositivoNotifier.NotificarCambioDispositivo(
                    dispositivoId,
                    result.Value,
                    cancellationToken);
            }
        }
    }

    /// <summary>
    /// Valida la secuencia entrada/salida del usuario
    /// </summary>
    /// <returns>(EsValido, MensajeError)</returns>
    private async Task<(bool EsValido, string MensajeError)> ValidarSecuenciaEntradaSalida(
        Guid usuarioId,
        bool esEntrada,
        CancellationToken cancellationToken)
    {
        // Buscar el último registro del usuario (solo los que afectan espacio = Autorizados)
        var ultimoRegistro = await context.RegistrosIngresosEgresos
            .Where(r => r.UsuarioId == usuarioId)
            .Join(context.EstadosRegistro,
                r => r.EstadoRegistroId,
                e => e.Id,
                (r, e) => new { Registro = r, Estado = e })
            .Where(re => re.Estado.AfectaEspacio) // Solo registros exitosos (Autorizados)
            .OrderByDescending(re => re.Registro.Fecha)
            .Select(re => re.Registro)
            .FirstOrDefaultAsync(cancellationToken);

        // Caso 1: No existe ningún registro previo exitoso
        if (ultimoRegistro is null)
        {
            if (!esEntrada)
            {
                return (false, "No tiene una entrada activa para registrar una salida. El primer registro debe ser una entrada.");
            }
            return (true, string.Empty);
        }

        // Determinar si el último registro fue entrada o salida
        bool ultimoFueEntrada = ultimoRegistro.PuntoEntradaId.HasValue && !ultimoRegistro.PuntoSalidaId.HasValue;
        bool ultimoFueSalida = !ultimoRegistro.PuntoEntradaId.HasValue && ultimoRegistro.PuntoSalidaId.HasValue;

        // Caso 2: Último registro fue ENTRADA
        if (ultimoFueEntrada)
        {
            if (esEntrada)
            {
                return (false, "Ya tiene una entrada activa. Debe registrar una salida primero.");
            }
            return (true, string.Empty);
        }

        // Caso 3: Último registro fue SALIDA
        if (ultimoFueSalida)
        {
            if (!esEntrada)
            {
                return (false, "No tiene una entrada activa para registrar una salida.");
            }
            return (true, string.Empty);
        }

        // Caso 4: Caso improbable (registro corrupto sin entrada ni salida)
        return (true, string.Empty);
    }
}