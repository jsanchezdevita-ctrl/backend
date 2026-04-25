using SharedKernel;

namespace Domain.Parametros;

public sealed class ParametroHistorial : Entity
{
    public Guid Id { get; set; }
    public string Llave { get; set; }
    public string Valor { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string ActualizadoPor { get; set; }
    public int Version { get; set; }
}