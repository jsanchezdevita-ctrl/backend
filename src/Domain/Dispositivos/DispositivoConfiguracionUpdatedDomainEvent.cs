using SharedKernel;

namespace Domain.Dispositivos;

public sealed record DispositivoConfiguracionUpdatedDomainEvent(Guid DispositivoId) : IDomainEvent;