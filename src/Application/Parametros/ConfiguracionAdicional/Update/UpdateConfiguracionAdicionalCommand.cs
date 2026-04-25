using Application.Abstractions.Messaging;

namespace Application.Parametros.ConfiguracionAdicional.Update;

public sealed record UpdateConfiguracionAdicionalCommand(
    string ZonaHoraria,
    int RetencionLogsDias,
    string FrecuenciaBackup) : ICommand;