using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Parametros;
using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;

namespace Application.Parametros.ConfiguracionAdicional.Update;

internal sealed class UpdateConfiguracionAdicionalCommandHandler(
    IApplicationDbContext context,
    IParametroValidator parametroValidator,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateConfiguracionAdicionalCommand>
{
    public async Task<Result> Handle(
        UpdateConfiguracionAdicionalCommand command,
        CancellationToken cancellationToken)
    {
        var parametro = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "configuracion_adicional",
                cancellationToken);

        if (parametro is null)
            return Result.Failure(ParametrosErrores.NotFoundByKey("configuracion_adicional"));

        var jsonAValidar = JsonSerializer.Serialize(new
        {
            zonaHoraria = command.ZonaHoraria,
            retencionLogsDias = command.RetencionLogsDias,
            frecuenciaBackup = command.FrecuenciaBackup
        });

        var validacionResult = await parametroValidator.ValidarJsonAsync(
            parametro.Llave,
            jsonAValidar);

        if (validacionResult.IsFailure)
        {
            return validacionResult;
        }

        // Crear historial antes de modificar
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

        // Actualizar con el nuevo objeto tipado
        parametro.Valor = JsonSerializer.Serialize(new
        {
            command.ZonaHoraria,
            command.RetencionLogsDias,
            command.FrecuenciaBackup
        });

        parametro.FechaActualizacion = dateTimeProvider.UtcNow;
        parametro.ActualizadoPor = userContext.Email;
        parametro.Version++;

        // Eventos de dominio
        parametro.Raise(new ParametroUpdatedDomainEvent(parametro.Id, parametro.Llave));
        historico.Raise(new ParametroHistorialRegisteredDomainEvent(historico.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}