using SharedKernel;

namespace Domain.ZonasRoles;

public sealed class ZonaRol : Entity
{
    public Guid Id { get; set; }
    public Guid ZonaId { get; set; }
    public Guid RolId { get; set; }
    public int CapacidadMaxima { get; set; }
    public int EspacioUtilizado { get; set; }
}