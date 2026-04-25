using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Parametros;
using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;

namespace Application.Parametros.SeguridadQr.Update;

internal sealed class UpdateSeguridadQrCommandHandler(
    IApplicationDbContext context,
    IParametroValidator parametroValidator,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateSeguridadQrCommand>
{
    public async Task<Result> Handle(
        UpdateSeguridadQrCommand command,
        CancellationToken cancellationToken)
    {
        var parametro = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "seguridad_qr",
                cancellationToken);

        if (parametro is null)
            return Result.Failure(ParametrosErrores.NotFoundByKey("seguridad_qr"));

        var jsonAValidar = JsonSerializer.Serialize(new
        {
            vigenciaQRHoras = command.VigenciaQRHoras,
            intervaloRenovacionMinutos = command.IntervaloRenovacionMinutos,
            nivelCifrado = command.NivelCifrado
        });

        var validacionResult = await parametroValidator.ValidarJsonAsync(
            parametro.Llave,
            jsonAValidar);

        if (validacionResult.IsFailure)
            return validacionResult;

        var historico = new ParametroHistorial
        {
            Id = Guid.NewGuid(),
            Llave = parametro.Llave,
            Valor = parametro.Valor,
            Descripcion = parametro.Descripcion,
            FechaActualizacion = parametro.FechaActualizacion,
            ActualizadoPor = parametro.ActualizadoPor,
            Version = parametro.Version
        };
        context.ParametrosHistorial.Add(historico);

        parametro.Valor = JsonSerializer.Serialize(new
        {
            command.VigenciaQRHoras,
            command.IntervaloRenovacionMinutos,
            command.NivelCifrado
        });

        parametro.FechaActualizacion = dateTimeProvider.UtcNow;
        parametro.ActualizadoPor = userContext.Email;
        parametro.Version++;

        parametro.Raise(new ParametroUpdatedDomainEvent(parametro.Id, parametro.Llave));
        historico.Raise(new ParametroHistorialRegisteredDomainEvent(historico.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}