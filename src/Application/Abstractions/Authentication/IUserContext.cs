namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UsuarioId { get; }

    string Email { get;}
}
