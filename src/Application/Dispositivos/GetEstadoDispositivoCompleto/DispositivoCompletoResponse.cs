namespace Application.Dispositivos.GetEstadoDispositivoCompleto;

public sealed record DispositivoCompletoResponse
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? DireccionIp { get; init; }
    public string? DispositivoId { get; init; }
    public PuntoControlInfoResponse PuntoControl { get; init; } = null!;
    public List<ZonaRolInfoResponse> ZonasRoles { get; init; } = new();
}

public sealed record PuntoControlInfoResponse
{
    public Guid PuntoControlId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string? Ubicacion { get; init; }
}

public sealed record ZonaRolInfoResponse
{
    public Guid ZonaRolId { get; init; }
    public Guid ZonaId { get; init; }
    public string ZonaNombre { get; init; } = string.Empty;
    public Guid RolId { get; init; }
    public string RolNombre { get; init; } = string.Empty;
    public int CapacidadMaxima { get; init; }
    public int EspacioUtilizado { get; init; }
    public int Disponible => CapacidadMaxima - EspacioUtilizado;
}