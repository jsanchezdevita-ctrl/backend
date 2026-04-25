using Domain.Enums;
using SharedKernel;

namespace Domain.Usuarios;

public sealed class Usuario : Entity
{
    public Guid Id { get; set; }
    public string NumeroDocumento { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string PasswordHash { get; set; }
    public UsuarioState Estado { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFin { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime FechaUltimaModificacion { get; set; }
    public string? FcmToken { get; set; }
    public DateTime? FcmTokenUpdatedAt { get; set; }
}
