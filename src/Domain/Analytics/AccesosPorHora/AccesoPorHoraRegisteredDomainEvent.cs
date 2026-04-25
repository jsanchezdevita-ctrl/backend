using SharedKernel;

namespace Domain.Analytics.AccesosPorHora;

public sealed record AccesoPorHoraRegisteredDomainEvent(Guid AccesoPorHoraId) : IDomainEvent;