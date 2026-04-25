using Domain.ZonasPuntosControl;
using SharedKernel;

namespace Domain.Zonas;

public sealed class Zona : Entity
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public ICollection<ZonaPuntoControl> PuntosDeControlAsociados { get; set; } = [];
}