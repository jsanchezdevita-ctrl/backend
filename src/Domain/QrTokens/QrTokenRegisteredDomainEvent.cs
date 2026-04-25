using SharedKernel;

namespace Domain.QrTokens;

public sealed record QrTokenRegisteredDomainEvent(Guid QrTokenId) : IDomainEvent;