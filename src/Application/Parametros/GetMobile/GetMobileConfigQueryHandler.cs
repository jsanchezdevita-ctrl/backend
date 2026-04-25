using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;

namespace Application.Parametros.GetMobile;

internal sealed class GetMobileConfigQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetMobileConfigQuery, MobileConfigResponse>
{
    public async Task<Result<MobileConfigResponse>> Handle(
        GetMobileConfigQuery query,
        CancellationToken cancellationToken)
    {
        var seguridadQr = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "seguridad_qr", cancellationToken);

        var politicasAcceso = await context.Parametros
            .FirstOrDefaultAsync(p => p.Llave == "politicas_acceso", cancellationToken);

        if (seguridadQr is null)
            return Result.Failure<MobileConfigResponse>(ParametrosErrores.NotFoundByKey("seguridad_qr"));

        if (politicasAcceso is null)
            return Result.Failure<MobileConfigResponse>(ParametrosErrores.NotFoundByKey("politicas_acceso"));

        try
        {
            var seguridadQrData = JsonSerializer.Deserialize<SeguridadQrData>(seguridadQr.Valor);
            var politicasData = JsonSerializer.Deserialize<PoliticasAccesoData>(politicasAcceso.Valor);

            if (seguridadQrData is null)
                return Result.Failure<MobileConfigResponse>(ParametrosErrores.InvalidJsonFormat("seguridad_qr"));

            if (politicasData is null)
                return Result.Failure<MobileConfigResponse>(ParametrosErrores.InvalidJsonFormat("politicas_acceso"));

            var response = new MobileConfigResponse(
                IntervaloRenovacionMinutos: seguridadQrData.IntervaloRenovacionMinutos,
                BloqueoAutomaticoPuertas: politicasData.BloqueoAutomaticoPuertas,
                TiempoBloqueoSegundos: politicasData.TiempoBloqueoSegundos
            );

            return Result.Success(response);
        }
        catch (JsonException)
        {
            return Result.Failure<MobileConfigResponse>(ParametrosErrores.InvalidJsonFormat("seguridad_qr"));
        }
    }

    private sealed record SeguridadQrData(
        int IntervaloRenovacionMinutos,
        int VigenciaQRHoras,
        string NivelCifrado);

    private sealed record PoliticasAccesoData(
        bool RegistroAccesos,
        bool NotificarAccesosNoAutorizados,
        bool BloqueoAutomaticoPuertas,
        int TiempoBloqueoSegundos);
}