using SharedKernel;

namespace Domain.ZonasRoles;

public sealed record ZonaRolAsignadoDomainEvent(Guid ZonaId, Guid RolId, int CapacidadMaxima) : IDomainEvent;

public sealed record EspacioZonaActualizadoDomainEvent(Guid ZonaRolId, int EspacioUtilizado) : IDomainEvent;