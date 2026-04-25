using Application.Abstractions.Messaging;

namespace Application.Analytics.AccesosTipoUsuario.GetWeeklySummary;

public sealed record GetAccesosTipoUsuarioWeeklySummaryQuery()
    : IQuery<List<AccesosTipoUsuarioSummaryResponse>>;