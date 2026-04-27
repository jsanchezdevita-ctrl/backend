using FluentValidation;

namespace Application.Usuarios.UpdatePassword;

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La nueva contraseña es requerida.")
            .MinimumLength(6).WithMessage("La nueva contraseña debe tener al menos 6 caracteres.");
    }
}
