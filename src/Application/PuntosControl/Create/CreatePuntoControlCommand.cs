using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.PuntosControl.Create;

public sealed record CreatePuntoControlCommand(
    string Nombre,
    string Ubicacion,
    PuntoControlType Tipo,
    string Descripcion) : ICommand<Guid>;