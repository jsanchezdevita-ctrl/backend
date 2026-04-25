using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Parametros;
using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;

namespace Application.Parametros.Autenticacion.Update;

internal sealed class UpdateAutenticacionCommandHandler(
    IApplicationDbContext context,
    IParametroValidator parametroValidator,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateAutenticacionCommand>
{
    public async Task<Result> Handle(
        UpdateAutenticacionCommand command,
        CancellationToken cancellationToken)
    {
        var parametro = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "autenticacion",
                cancellationToken);

        if (parametro is null)
            return Result.Failure(ParametrosErrores.NotFoundByKey("autenticacion"));

        var jsonAValidar = JsonSerializer.Serialize(new
        {
            autenticacionDosFactores = command.AutenticacionDosFactores,
            tiempoSesionMinutos = command.TiempoSesionMinutos,
            intentosMaximosLogin = command.IntentosMaximosLogin,
            bloquearCuentaDespuesIntentos = command.BloquearCuentaDespuesIntentos
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
            command.AutenticacionDosFactores,
            command.TiempoSesionMinutos,
            command.IntentosMaximosLogin,
            command.BloquearCuentaDespuesIntentos
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