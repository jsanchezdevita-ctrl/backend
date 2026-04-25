using SharedKernel;

namespace Domain.Parametros;

public sealed class ParametroSchema : Entity
{
    public Guid Id { get; set; }
    public string Llave { get; set; }
    public string Schema { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}