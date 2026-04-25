using Application.Abstractions.Messaging;

namespace Application.Parametros.SeguridadQr.Update;

public sealed record UpdateSeguridadQrCommand(
    int VigenciaQRHoras,
    int IntervaloRenovacionMinutos,
    string NivelCifrado) : ICommand;