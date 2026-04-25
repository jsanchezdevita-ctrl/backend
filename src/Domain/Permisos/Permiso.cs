using SharedKernel;

namespace Domain.Permisos;

public sealed class Permiso : Entity
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
}