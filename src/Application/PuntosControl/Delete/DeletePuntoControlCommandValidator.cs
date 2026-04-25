using FluentValidation;

namespace Application.PuntosControl.Delete;

internal sealed class DeletePuntoControlCommandValidator : AbstractValidator<DeletePuntoControlCommand>
{
    public DeletePuntoControlCommandValidator()
    {
        RuleFor(c => c.PuntoControlId)
            .NotEmpty().WithMessage("El ID del punto de control es requerido");
    }
}