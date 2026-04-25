using Application.Abstractions.Messaging;
using Application.Usuarios.Register;
using Domain.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Usuarios;

internal sealed class Register : IEndpoint
{
    public sealed record Request(
        string NumeroDocumento,
        string Email, 
        string Nombre, 
        string Apellido, 
        string Password, 
        UsuarioState Estado,
        List<Guid> RolIds,
        TimeSpan HorarioInicio,
        TimeSpan HorarioFin);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("usuarios/register", async (
            Request request,
            ICommandHandler<RegisterUsuarioCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUsuarioCommand(
                request.NumeroDocumento,
                request.Email,
                request.Nombre,
                request.Apellido,
                request.Password,
                request.Estado,
                request.RolIds,
                request.HorarioInicio,
                request.HorarioFin);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                id => Results.Created($"/usuarios/{id}", id),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .HasPermission(Permissions.Admin)
        .WithTags(Tags.Usuarios);
    }
}
