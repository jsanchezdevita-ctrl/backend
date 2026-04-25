using SharedKernel;

namespace Domain.Parametros;

public sealed record ParametroSchemaRegisteredDomainEvent(Guid SchemaId) : IDomainEvent;