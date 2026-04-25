using SharedKernel;

namespace Domain.RolesPermisos;

public sealed class RolPermiso : Entity
{
    public Guid Id { get; set; }
    public Guid PermisoId { get; set; }
    public Guid RolId { get; set; }
}