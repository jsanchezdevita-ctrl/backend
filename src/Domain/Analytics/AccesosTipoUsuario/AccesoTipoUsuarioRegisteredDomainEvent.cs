using SharedKernel;

namespace Domain.Analytics.AccesosTipoUsuario;

public sealed record AccesoTipoUsuarioRegisteredDomainEvent(Guid AccesoTipoUsuarioId) : IDomainEvent;