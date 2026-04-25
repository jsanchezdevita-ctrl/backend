using SharedKernel;

namespace Domain.RegistrosIngresosEgresos;

public sealed record RegistroIngresoEgresoRegisteredDomainEvent(Guid RegistroId) : IDomainEvent;
