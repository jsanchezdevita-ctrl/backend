using FluentValidation;

namespace Application.PuntosControl.Create;

internal sealed class CreatePuntoControlCommandValidator : AbstractValidator<CreatePuntoControlCommand>
{
    public CreatePuntoControlCommandValidator()
    {
        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(c => c.Ubicacion)
            .NotEmpty().WithMessage("La ubicación es requerida")
            .MaximumLength(100).WithMessage("La ubicación no puede exceder 100 caracteres");

        RuleFor(c => c.Tipo)
            .IsInEnum().WithMessage("El tipo debe ser Entrada o Salida");

        RuleFor(c => c.Descripcion)
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres")
            .When(c => !string.IsNullOrEmpty(c.Descripcion));
    }
}