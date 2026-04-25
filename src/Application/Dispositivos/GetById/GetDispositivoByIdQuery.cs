using Application.Abstractions.Messaging;

namespace Application.Dispositivos.GetById;

public sealed record GetDispositivoByIdQuery(Guid DispositivoId) : IQuery<DispositivoResponse>;

public record DispositivoResponse(
    Guid Id,
    string DispositivoId,
    string Nombre,
    string DireccionIp,
    DateTime? UltimaConexion,
    Guid PuntoControlId,
    DispositivoConfiguracionResponse? Configuracion);

public record DispositivoConfiguracionResponse(
    int FrecuenciaSincronizacionSegundos,
    string PotenciaTransmision,
    int CanalComunicacion);