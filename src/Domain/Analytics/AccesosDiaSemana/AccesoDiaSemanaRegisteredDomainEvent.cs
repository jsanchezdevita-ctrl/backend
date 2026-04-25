using SharedKernel;

namespace Domain.Analytics.AccesosDiaSemana;

public sealed record AccesoDiaSemanaRegisteredDomainEvent(Guid AccesoDiaSemanaId) : IDomainEvent;