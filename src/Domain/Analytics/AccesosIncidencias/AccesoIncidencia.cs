using SharedKernel;

namespace Domain.Analytics.AccesosIncidencias;

public sealed class AccesoIncidencia : AnalyticsEntity
{
    public Guid Id { get; set; }
    public Guid PuntoControlId { get; set; }
    public string NombrePuntoControl { get; set; }
    public string Incidencia { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; }
}