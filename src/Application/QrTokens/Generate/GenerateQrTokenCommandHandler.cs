using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.QrTokens;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;

namespace Application.QrTokens.Generate;

internal sealed class GenerateQrTokenCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<GenerateQrTokenCommand, QrTokenResponse>
{
    public async Task<Result<QrTokenResponse>> Handle(
        GenerateQrTokenCommand command,
        CancellationToken cancellationToken)
    {
        var seguridadQR = await context.Parametros.FirstOrDefaultAsync(p => p.Llave == "seguridad_qr",cancellationToken);

        if (seguridadQR is null)
        {
            return Result.Failure<QrTokenResponse>(QrTokenErrors.UserNotFound(command.UsuarioId));
        }

        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == command.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            return Result.Failure<QrTokenResponse>(QrTokenErrors.SeguridadQRNotFound());
        }

        var rol = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == command.RolId, cancellationToken);

        if (rol is null)
        {
            return Result.Failure<QrTokenResponse>(QrTokenErrors.RolNotFound(command.RolId));
        }

        var usuarioRol = await context.UsuariosRoles
            .FirstOrDefaultAsync(ur => ur.UsuarioId == command.UsuarioId && 
                                       ur.RolId == command.RolId, cancellationToken);

        if (usuarioRol is null)
        {
            return Result.Failure<QrTokenResponse>(
                QrTokenErrors.UsuarioRolNotFound(command.UsuarioId,command.RolId));
        }

        string token = GenerateShortToken();

        using var doc = JsonDocument.Parse(seguridadQR.Valor);

        var vigencia = doc.RootElement
            .GetProperty("VigenciaQRHoras")
            .GetInt32();

        var qrToken = new QrToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UsuarioId = command.UsuarioId,
            FechaCreacion = dateTimeProvider.UtcNow,
            FechaExpiracion = dateTimeProvider.UtcNow.AddHours(vigencia),
            RolId = command.RolId,
        };

        context.QrTokens.Add(qrToken);
        await context.SaveChangesAsync(cancellationToken);

        return new QrTokenResponse(token, qrToken.FechaExpiracion);
    }

    private static string GenerateShortToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("=", "")
            .Replace("+", "")
            .Replace("/", "")
            .Substring(0, 10);
    }
}