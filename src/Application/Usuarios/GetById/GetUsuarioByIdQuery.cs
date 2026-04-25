using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Usuarios.GetById;

public sealed record GetUsuarioByIdQuery(Guid UsuarioId)
    : IQuery<UsuarioByIdResponse>;

public sealed record UsuarioByIdResponse(
    string Email,
    string Nombre,
    string Apellido,
    List<ItemResponse<Guid>> Roles,
    string NumeroDocumento,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFin,
    ItemResponse<int> Estado);