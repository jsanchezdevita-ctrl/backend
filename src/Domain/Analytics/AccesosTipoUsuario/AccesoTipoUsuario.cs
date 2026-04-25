using SharedKernel;

namespace Domain.Analytics.AccesosTipoUsuario;

public sealed class AccesoTipoUsuario : AnalyticsEntity
{
    public Guid Id { get; set; }
    public int Cantidad { get; set; }
    public string TipoUsuario { get; set; }
    public DateTime Fecha { get; set; }
}