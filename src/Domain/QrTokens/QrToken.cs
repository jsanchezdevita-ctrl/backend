using SharedKernel;

namespace Domain.QrTokens;

public sealed class QrToken : Entity
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public DateTime? FechaUso { get; set; }
    public Guid? DispositivoId { get; set; }
    public Guid? PuntoControlId { get; set; }
    public Guid? ZonaId { get; set; }
    public Guid? RegistroIngresoId { get; set; }
    public Guid RolId { get; set; }
}