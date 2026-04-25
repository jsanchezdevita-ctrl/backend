using SharedKernel;

namespace Domain.UsuariosRoles;

public sealed class UsuarioRol : Entity
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid RolId { get; set; }
    public DateTime FechaAsignacion { get; set; }
}
