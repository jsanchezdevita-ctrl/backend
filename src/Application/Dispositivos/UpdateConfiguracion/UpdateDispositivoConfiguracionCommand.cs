using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Dispositivos.UpdateConfiguracion;

public sealed record UpdateDispositivoConfiguracionCommand(
    Guid DispositivoId,
    int FrecuenciaSincronizacionSegundos,
    DispositivoPowerTransmission PotenciaTransmision,
    int CanalComunicacion) : ICommand;