using FluentValidation;

namespace Application.PuntosControl.Update;

internal sealed class UpdatePuntoControlCommandValidator : AbstractValidator<UpdatePuntoControlCommand>
{
    public UpdatePuntoControlCommandValidator()
    {
        RuleFor(c => c.PuntoControlId)
            .NotEmpty().WithMessage("El ID del punto de control es requerido");

        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(c => c.Ubicacion)
            .NotEmpty().WithMessage("La ubicación es requerida")
            .MaximumLength(100).WithMessage("La ubicación no puede exceder 100 caracteres");

        RuleFor(c => c.Tipo)
            .IsInEnum().WithMessage("El tipo debe ser Entrada o Salida");

        RuleFor(c => c.Estado)
            .IsInEnum().WithMessage("El estado debe ser Activo o Inactivo");

        RuleFor(c => c.Descripcion)
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres")
            .When(c => !string.IsNullOrEmpty(c.Descripcion));
    }
}