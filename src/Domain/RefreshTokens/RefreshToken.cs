using SharedKernel;

namespace Domain.Authentication.RefreshTokens;

public sealed class RefreshToken : Entity
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Token { get; set; } = default!;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public DateTime? FechaRevocacion { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsRevoked => FechaRevocacion.HasValue;

    public bool IsExpired => FechaExpiracion <= DateTime.UtcNow;

    public void Revoke(string? replacedByToken = null)
    {
        FechaRevocacion = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}
