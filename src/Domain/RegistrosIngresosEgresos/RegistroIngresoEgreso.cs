using SharedKernel;

namespace Domain.RegistrosIngresosEgresos;

public sealed class RegistroIngresoEgreso : Entity
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? PuntoEntradaId { get; set; }
    public Guid? PuntoSalidaId { get; set; }
    public Guid EstadoRegistroId { get; set; }
    public Guid? ZonaId { get; set; }
    public Guid? RolId { get; set; }
    public String? Observacion { get; set; }
}
