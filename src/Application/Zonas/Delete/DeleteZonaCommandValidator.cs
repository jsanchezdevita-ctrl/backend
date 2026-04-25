using FluentValidation;

namespace Application.Zonas.Delete;

internal sealed class DeleteZonaCommandValidator : AbstractValidator<DeleteZonaCommand>
{
    public DeleteZonaCommandValidator()
    {
        RuleFor(c => c.ZonaId)
            .NotEmpty().WithMessage("El ID de la zona es requerido");
    }
}