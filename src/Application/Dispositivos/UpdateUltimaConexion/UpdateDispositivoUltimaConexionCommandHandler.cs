using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Dispositivos;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Dispositivos.UpdateUltimaConexion;

internal sealed class UpdateDispositivoUltimaConexionCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateDispositivoUltimaConexionCommand>
{
    public async Task<Result> Handle(
        UpdateDispositivoUltimaConexionCommand command,
        CancellationToken cancellationToken)
    {
        var dispositivo = await context.Dispositivos
            .FirstOrDefaultAsync(d => d.Id == command.DispositivoId, cancellationToken);

        if (dispositivo is null)
        {
            return Result.Failure(DispositivosErrores.NotFound(command.DispositivoId));
        }

        dispositivo.UltimaConexion = dateTimeProvider.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}