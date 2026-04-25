using SharedKernel;

namespace Domain.Roles;

public sealed class Rol : Entity
{
    public Guid Id { get; set; }
    public string NombreRol { get; set; }
    public string Descripcion { get; set; }
    public bool EsAdmin { get; set; }
}
