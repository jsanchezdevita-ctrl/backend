namespace Application.RegistrosIngresosEgresos.GetForMonitoring;

public sealed record RegistroIngresoEgresoMonitoringResponse(
    Guid Id,
    string FechaHora,
    UsuarioInfo Usuario,
    TipoInfo Tipo,
    PuntoAccesoInfo PuntoAcceso,
    string Ubicacion,
    EstadoInfo Estado,
    String Observacion);

public sealed record UsuarioInfo(Guid Id, string NombreCompleto, string NumeroDocumento);
public sealed record TipoInfo(Guid Id, string Nombre);
public sealed record PuntoAccesoInfo(Guid Id, string Nombre);
public sealed record EstadoInfo(Guid Id, string Descripcion);