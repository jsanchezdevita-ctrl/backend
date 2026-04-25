using SharedKernel;

namespace Domain.Analytics.AccesosDiaSemana;

public sealed class AccesoDiaSemana : AnalyticsEntity
{
    public Guid Id { get; set; }
    public int Cantidad { get; set; }
    public string DiaSemanaCompleto { get; set; }
    public string DiaSemanaCorto { get; set; }
    public DateTime Fecha { get; set; }
}