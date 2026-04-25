using FluentValidation;

namespace Application.Usuarios.UpdateFcmToken;

internal sealed class UpdateUsuarioFcmTokenCommandValidator
    : AbstractValidator<UpdateUsuarioFcmTokenCommand>
{
    public UpdateUsuarioFcmTokenCommandValidator()
    {
        RuleFor(c => c.FcmToken)
            .NotEmpty()
            .WithMessage("El token FCM es requerido")
            .MaximumLength(500)
            .WithMessage("El token FCM no puede exceder 500 caracteres");
    }
}