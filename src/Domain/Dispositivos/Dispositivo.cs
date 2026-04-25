using SharedKernel;

namespace Domain.Dispositivos;

public sealed class Dispositivo : Entity
{
    public Guid Id { get; set; }
    public string DispositivoId { get; set; }
    public string Nombre { get; set; }
    public string DireccionIp { get; set; }
    public DateTime? UltimaConexion { get; set; }
    public Guid PuntoControlId { get; set; }
}