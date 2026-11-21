using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Application.RegistrosIngresosEgresos.Create;
using Domain.Enums;
using Domain.PuntosControl;
using Domain.QrTokens;
using Domain.RegistrosIngresosEgresos;
using Domain.ZonasRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.QrTokens.Validate;

internal sealed class ValidateQrTokenCommandHandler(
    IApplicationDbContext context,
    ICommandHandler<CreateRegistroIngresoEgresoCommand, Guid> createRegistroHandler,
    IFcmNotificationService fcmNotificationService,
    IDateTimeProvider dateTimeProvider,
    ILogger<ValidateQrTokenCommandHandler> logger)
    : ICommandHandler<ValidateQrTokenCommand, ValidateQrTokenResponse>
{
    public async Task<Result<ValidateQrTokenResponse>> Handle(
        ValidateQrTokenCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Buscar QrToken (FATAL - sin QR no podemos continuar)
        var qrToken = await context.QrTokens
            .FirstOrDefaultAsync(q => q.Token == command.QrToken, cancellationToken);

        if (qrToken is null)
            return Result.Failure<ValidateQrTokenResponse>(QrTokenErrors.NotFound(command.QrToken));

        if (qrToken.FechaUso.HasValue)
            return Result.Failure<ValidateQrTokenResponse>(QrTokenErrors.AlreadyUsed);

        if (qrToken.FechaExpiracion < dateTimeProvider.UtcNow)
            return Result.Failure<ValidateQrTokenResponse>(QrTokenErrors.Expired);

        // 2. Buscar dispositivo (FATAL - sin dispositivo no sabemos punto de control)
        var dispositivo = await context.Dispositivos
            .FirstOrDefaultAsync(d => d.DispositivoId == command.DispositivoId, cancellationToken);

        if (dispositivo is null)
            return Result.Failure<ValidateQrTokenResponse>(QrTokenErrors.DispositivoNotFound(command.DispositivoId));

        var puntoControlId = dispositivo.PuntoControlId;
        var puntoControl = await context.PuntosControl
            .FirstOrDefaultAsync(p => p.Id == puntoControlId, cancellationToken);

        if (puntoControl is null)
            return Result.Failure<ValidateQrTokenResponse>(RegistroIngresoEgresoErrors
                .PuntoControlNotFound(puntoControlId, "control"));

        bool esEntrada = puntoControl.Tipo == PuntoControlType.Entrada;

        // 3. Buscar Zona válida para el RolId del QR
        var zonaRol = await EncontrarZonaRolValidaAsync(
            qrToken.UsuarioId,
            qrToken.RolId,
            puntoControlId,
            esEntrada,
            cancellationToken);

        // Ya no retornamos error si no hay zona
        // Dejamos que el CreateRegistroHandler maneje la denegación
        Guid? zonaRolId = zonaRol?.Id;
        string? observacion = null;

        if (zonaRol is null)
        {
            observacion = esEntrada
                ? "No hay zona disponible con capacidad para entrada"
                : "No se encontró una entrada activa en una zona con este punto de salida";

            logger.LogInformation("QR validado pero sin zona disponible: {QrToken}, Motivo: {Observacion}",
                command.QrToken, observacion);
        }

        // 4. Determinar estado del registro (siempre Autorizado, el handler decidirá)
        Guid estadoRegistroId = await DeterminarEstadoRegistroAsync(cancellationToken);

        // 5. Crear registro de ingreso/egreso
        var createCommand = new CreateRegistroIngresoEgresoCommand(
            UsuarioId: qrToken.UsuarioId,
            PuntoEntradaId: esEntrada ? puntoControlId : null,
            PuntoSalidaId: !esEntrada ? puntoControlId : null,
            EstadoRegistroId: estadoRegistroId,  // Siempre enviamos Autorizado
            ZonaRolId: zonaRolId,  // Puede ser null
            Observacion: observacion);  // Pasamos observación si aplica

        var result = await createRegistroHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return Result.Failure<ValidateQrTokenResponse>(result.Error);

        var registro = await context.RegistrosIngresosEgresos.Where(x => x.Id == result.Value).FirstOrDefaultAsync();
        bool estadoRegistroFinal = false;

        if (registro is not null) 
        {
            estadoRegistroFinal = await DeterminarEstadoRegistroByIdAsync(registro.EstadoRegistroId, cancellationToken);
        }


        // 6. Actualizar QrToken con los datos usados (aunque sea denegado)
        qrToken.FechaUso = dateTimeProvider.UtcNow;
        qrToken.DispositivoId = dispositivo.Id;
        qrToken.PuntoControlId = puntoControlId;
        qrToken.ZonaId = zonaRol?.ZonaId;  // Puede ser null
        qrToken.RegistroIngresoId = result.Value;

        await context.SaveChangesAsync(cancellationToken);

        await SendNotificationToUserAsync(qrToken.UsuarioId, puntoControl, estadoRegistroFinal, cancellationToken);

        return new ValidateQrTokenResponse 
        (
            result.Value,
            estadoRegistroFinal
        );
    }

    private async Task<ZonaRol?> EncontrarZonaRolValidaAsync(
        Guid usuarioId,
        Guid rolId,
        Guid puntoControlId,
        bool esEntrada,
        CancellationToken cancellationToken)
    {
        if (!esEntrada)
        {
            return await EncontrarZonaRolParaSalidaAsync(usuarioId, rolId, puntoControlId, cancellationToken);
        }

        // Buscar zonas donde el rol tiene acceso y que tengan este punto de control,
        // ordenadas por nombre de zona para que la asignación entre zonas candidatas sea determinística
        var zonasCandidatas = await context.ZonasRoles
            .Where(zr => zr.RolId == rolId && !zr.Deleted)
            .Join(context.ZonasPuntosControl
                .Where(zpc => zpc.PuntoControlId == puntoControlId && !zpc.Deleted),
                zr => zr.ZonaId,
                zpc => zpc.ZonaId,
                (zr, _) => zr)
            .Join(context.Zonas,
                zr => zr.ZonaId,
                z => z.Id,
                (zr, z) => new { ZonaRol = zr, z.Nombre })
            .OrderBy(x => x.Nombre)
            .Select(x => x.ZonaRol)
            .ToListAsync(cancellationToken);

        // Para entrada: tomar la primera zona (en orden alfabético) con espacio disponible
        foreach (var zonaRol in zonasCandidatas)
        {
            if (zonaRol.EspacioUtilizado < zonaRol.CapacidadMaxima)
            {
                return zonaRol;
            }
        }

        return null;
    }

    private async Task<ZonaRol?> EncontrarZonaRolParaSalidaAsync(
        Guid usuarioId,
        Guid rolId,
        Guid puntoControlId,
        CancellationToken cancellationToken)
    {
        // La salida debe registrarse en la misma zona donde el usuario tiene la entrada activa,
        // no en cualquier zona candidata con el punto de control compartido.
        var ultimoRegistro = await context.RegistrosIngresosEgresos
            .Where(r => r.UsuarioId == usuarioId)
            .Join(context.EstadosRegistro,
                r => r.EstadoRegistroId,
                e => e.Id,
                (r, e) => new { Registro = r, Estado = e })
            .Where(re => re.Estado.AfectaEspacio)
            .OrderByDescending(re => re.Registro.Fecha)
            .Select(re => re.Registro)
            .FirstOrDefaultAsync(cancellationToken);

        bool tieneEntradaActiva = ultimoRegistro is not null
            && ultimoRegistro.PuntoEntradaId.HasValue
            && !ultimoRegistro.PuntoSalidaId.HasValue
            && ultimoRegistro.ZonaId.HasValue;

        if (!tieneEntradaActiva)
        {
            return null;
        }

        return await context.ZonasRoles
            .Where(zr => zr.ZonaId == ultimoRegistro!.ZonaId && zr.RolId == rolId && !zr.Deleted)
            .Join(context.ZonasPuntosControl
                .Where(zpc => zpc.PuntoControlId == puntoControlId && !zpc.Deleted),
                zr => zr.ZonaId,
                zpc => zpc.ZonaId,
                (zr, _) => zr)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<Guid> DeterminarEstadoRegistroAsync(
        CancellationToken cancellationToken)
    {
        var estadoAutorizado = await context.EstadosRegistro
            .FirstOrDefaultAsync(e => e.Descripcion == "Autorizado", cancellationToken);

        if (estadoAutorizado is null)
            throw new InvalidOperationException("Estado 'Autorizado' no configurado en la base de datos");

        return estadoAutorizado.Id;
    }

    private async Task<bool> DeterminarEstadoRegistroByIdAsync(
        Guid id,CancellationToken cancellationToken)
    {
        var estadoAutorizado = await context.EstadosRegistro
            .FirstOrDefaultAsync(e => e.Id == id && e.Descripcion == "Autorizado", cancellationToken);

        if (estadoAutorizado is null)
            return false;

        return true;
    }

    private async Task SendNotificationToUserAsync(
        Guid usuarioId,
        PuntoControl puntoControl,
        bool estadoRegistro,
        CancellationToken cancellationToken)
    {
        try
        {
            // Obtener el token FCM del usuario desde la BD
            var usuarioConToken = await context.Usuarios
                .Where(u => u.Id == usuarioId && !string.IsNullOrEmpty(u.FcmToken))
                .Select(u => new { u.FcmToken, u.Nombre, u.Apellido })
                .FirstOrDefaultAsync(cancellationToken);

            if (usuarioConToken == null)
            {
                logger.LogInformation("Usuario {UsuarioId} no tiene token FCM configurado", usuarioId);
                return;
            }

            string tipoAcceso = puntoControl.Tipo == PuntoControlType.Entrada ? "Entrada" : "Salida";
            string nombrePunto = puntoControl.Nombre ?? "punto de control";
            string nombreUsuario = $"{usuarioConToken.Nombre} {usuarioConToken.Apellido}".Trim();

            var notificationResult = await fcmNotificationService.SendNotificationAsync(
                fcmToken: usuarioConToken.FcmToken,
                title: estadoRegistro ? $"Acceso Registrado - {tipoAcceso}" : $"Acceso Rechazado - {tipoAcceso}",
                body: $"Se registró su {tipoAcceso.ToLower()} en {nombrePunto}",
                type: estadoRegistro ? "success" : "denied",
                cancellationToken);

            if (notificationResult.IsFailure)
            {
                logger.LogWarning(
                    "No se pudo enviar notificación FCM al usuario {UsuarioId} ({Nombre}): {Error}",
                    usuarioId, nombreUsuario, notificationResult.Error);
            }
            else
            {
                logger.LogInformation(
                    "Notificación FCM enviada exitosamente al usuario {UsuarioId} ({Nombre})",
                    usuarioId, nombreUsuario);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error al enviar notificación FCM al usuario {UsuarioId}",
                usuarioId);
        }
    }
}