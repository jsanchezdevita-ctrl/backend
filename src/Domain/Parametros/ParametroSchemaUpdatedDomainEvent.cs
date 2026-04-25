using SharedKernel;

namespace Domain.Parametros;

public sealed record ParametroSchemaUpdatedDomainEvent(Guid SchemaId) : IDomainEvent;