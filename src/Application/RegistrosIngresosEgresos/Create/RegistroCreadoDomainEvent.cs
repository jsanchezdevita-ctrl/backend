using SharedKernel;

namespace Application.RegistrosIngresosEgresos.Create;

public sealed record RegistroCreadoDomainEvent(Guid RegistroId) : IDomainEvent;