using Application.Zonas.GetZonasEstadoWeb;

namespace Application.Abstractions.Notifications;

public interface IZonaNotifierWeb
{
    Task NotificarCambioZonas(
        Guid zonaIdModificada, 
        List<ZonaEstadoWebResponse> todasLasZonas, 
        CancellationToken cancellationToken = default);
}