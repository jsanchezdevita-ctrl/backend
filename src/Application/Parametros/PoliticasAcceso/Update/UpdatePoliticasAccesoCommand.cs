using Application.Abstractions.Messaging;

namespace Application.Parametros.PoliticasAcceso.Update;

public sealed record UpdatePoliticasAccesoCommand(
    bool RegistroAccesos,
    bool NotificarAccesosNoAutorizados,
    bool BloqueoAutomaticoPuertas,
    int TiempoBloqueoSegundos) : ICommand;