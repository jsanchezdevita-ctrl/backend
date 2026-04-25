using Application.Abstractions.Messaging;

namespace Application.Zonas.Delete;

public sealed record DeleteZonaCommand(Guid ZonaId) : ICommand;