using FluentValidation;

namespace Application.Dispositivos.Delete;

internal sealed class DeleteDispositivoCommandValidator : AbstractValidator<DeleteDispositivoCommand>
{
    public DeleteDispositivoCommandValidator()
    {
        RuleFor(c => c.DispositivoId)
            .NotEmpty().WithMessage("El ID del dispositivo es requerido");
    }
}