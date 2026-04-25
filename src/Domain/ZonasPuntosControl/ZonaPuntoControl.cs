using Domain.PuntosControl;
using Domain.Zonas;
using SharedKernel;

namespace Domain.ZonasPuntosControl;

public sealed class ZonaPuntoControl : Entity
{
    public Guid Id { get; set; }
    public Guid ZonaId { get; set; }
    public Zona Zona { get; set; } = null!;
    public Guid PuntoControlId { get; set; }
    public PuntoControl PuntoControl { get; set; } = null!;
}