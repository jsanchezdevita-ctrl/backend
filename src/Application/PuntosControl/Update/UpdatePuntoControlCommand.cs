using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.PuntosControl.Update;

public sealed record UpdatePuntoControlCommand(
    Guid PuntoControlId,
    string Nombre,
    string Ubicacion,
    PuntoControlType Tipo,
    PuntoControlState Estado,
    string Descripcion) : ICommand;