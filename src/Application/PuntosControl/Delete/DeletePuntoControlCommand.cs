using Application.Abstractions.Messaging;

namespace Application.PuntosControl.Delete;

public sealed record DeletePuntoControlCommand(Guid PuntoControlId) : ICommand;