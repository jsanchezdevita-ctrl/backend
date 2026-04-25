using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Dispositivos.GetAll;

public sealed record GetAllDispositivosQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<DispositivoResponse>>;

public record DispositivoResponse(
    Guid Id,
    string DispositivoId,
    string Nombre,
    string DireccionIp,
    DateTime? UltimaConexion,
    PuntoControlInfo PuntoControl,
    DispositivoConfiguracionResponse? Configuracion);

public record PuntoControlInfo(
    Guid Id,
    string Nombre,
    string Ubicacion,
    string Tipo);

public record DispositivoConfiguracionResponse(
    int FrecuenciaSincronizacionSegundos,
    string PotenciaTransmision,
    int CanalComunicacion);