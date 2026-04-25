using SharedKernel;

namespace Domain.Dispositivos;

public sealed record DispositivoRegisteredDomainEvent(Guid DispositivoId) : IDomainEvent;