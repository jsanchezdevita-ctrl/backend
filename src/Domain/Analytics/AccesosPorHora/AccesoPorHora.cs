using SharedKernel;

namespace Domain.Analytics.AccesosPorHora;

public sealed class AccesoPorHora : AnalyticsEntity
{
    public Guid Id { get; set; }
    public int Cantidad { get; set; }
    public int Hora { get; set; }
    public DateTime Fecha { get; set; }
}