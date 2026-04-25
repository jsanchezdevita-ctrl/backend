using Application.Zonas.GetZonasEstadoMobile;

namespace Application.Abstractions.Notifications;

public interface IZonaNotifier
{
    Task NotificarCambioZonas(
        Guid rolId, 
        Guid usuarioId, 
        List<ZonaEstadoMobileResponse> zonas, 
        CancellationToken cancellationToken = default);
}