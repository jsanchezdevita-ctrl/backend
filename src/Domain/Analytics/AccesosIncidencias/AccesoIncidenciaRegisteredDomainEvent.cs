using SharedKernel;

namespace Domain.Analytics.AccesosIncidencias;

public sealed record AccesoIncidenciaRegisteredDomainEvent(Guid AccesoIncidenciaId) : IDomainEvent;