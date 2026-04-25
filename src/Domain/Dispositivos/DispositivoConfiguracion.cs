using Domain.Enums;
using SharedKernel;

namespace Domain.Dispositivos;

public sealed class DispositivoConfiguracion : Entity
{
    public Guid Id { get; set; }
    public Guid DispositivoId { get; set; }
    public int FrecuenciaSincronizacionSegundos { get; set; }
    public DispositivoPowerTransmission PotenciaTransmision { get; set; }
    public int CanalComunicacion { get; set; }
}