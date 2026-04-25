using SharedKernel;

namespace Domain.EstadosRegistro;

public sealed class EstadoRegistro : Entity
{
    public Guid Id { get; set; }
    public string Descripcion { get; set; }
    public bool AfectaEspacio { get; set; }
}