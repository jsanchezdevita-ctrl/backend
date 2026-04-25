using Application.Abstractions.Messaging;

namespace Application.Parametros.Autenticacion.Update;

public sealed record UpdateAutenticacionCommand(
    bool AutenticacionDosFactores,
    int TiempoSesionMinutos,
    int IntentosMaximosLogin,
    bool BloquearCuentaDespuesIntentos) : ICommand;