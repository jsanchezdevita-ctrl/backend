using FluentValidation;

namespace Application.QrTokens.Validate;

internal sealed class ValidateQrTokenCommandValidator
    : AbstractValidator<ValidateQrTokenCommand>
{
    public ValidateQrTokenCommandValidator()
    {
        RuleFor(c => c.QrToken)
            .NotEmpty().WithMessage("El token QR es requerido")
            .MaximumLength(50).WithMessage("El token QR no puede exceder 50 caracteres");

        RuleFor(c => c.DispositivoId)
            .NotEmpty().WithMessage("El ID del dispositivo es requerido")
            .MaximumLength(50).WithMessage("El ID del dispositivo no puede exceder 50 caracteres");
    }
}