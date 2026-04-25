using Application.Abstractions.Messaging;

namespace Application.RegistrosIngresosEgresos.GetByUsuario;

public sealed record GetRegistrosByUsuarioQuery(Guid UsuarioId)
    : IQuery<List<RegistroIngresoEgresoUsuarioResponse>>;

public sealed record RegistroIngresoEgresoUsuarioResponse(
    Guid Id,
    DateTime FechaHora,
    string HaceTiempo,
    TipoRegistroInfo TipoRegistro,
    PuntoAccesoInfo PuntoAcceso,
    string Ubicacion,
    EstadoInfo Estado);

public sealed record TipoRegistroInfo(string Nombre); // "Ingreso" o "Egreso"
public sealed record PuntoAccesoInfo(Guid Id, string Nombre);
public sealed record EstadoInfo(Guid Id, string Descripcion);