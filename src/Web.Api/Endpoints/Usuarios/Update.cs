using Application.Abstractions.Messaging;
using Application.Usuarios.Update;
using Domain.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string NumeroDocumento,
        string Email,
        string Nombre,
        string Apellido,
        UsuarioState Estado,
        List<Guid> RolIds,
        TimeSpan HorarioInicio,
        TimeSpan HorarioFin);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("usuarios/{usuarioId}", async (
            Guid usuarioId,
            Request request,
            ICommandHandler<UpdateUsuarioCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateUsuarioCommand(
                usuarioId,
                request.NumeroDocumento,
                request.Email,
                request.Nombre,
                request.Apellido,
                request.Estado,
                request.RolIds,
                request.HorarioInicio,
                request.HorarioFin);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}