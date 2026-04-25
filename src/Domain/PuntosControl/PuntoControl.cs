using Domain.Enums;
using Domain.ZonasPuntosControl;
using SharedKernel;

namespace Domain.PuntosControl;

public sealed class PuntoControl : Entity
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Ubicacion { get; set; }
    public PuntoControlType Tipo { get; set; }
    public PuntoControlState Estado { get; set; }
    public string Descripcion { get; set; }
    public ICollection<ZonaPuntoControl> ZonasAsociadas { get; set; } = [];
}
