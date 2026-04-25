using FluentValidation;

namespace Application.QrTokens.Generate;

internal sealed class GenerateQrTokenCommandValidator
    : AbstractValidator<GenerateQrTokenCommand>
{
    public GenerateQrTokenCommandValidator()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario es requerido");

        RuleFor(c => c.RolId)
            .NotEmpty().WithMessage("El ID del rol es requerido");

    }
}