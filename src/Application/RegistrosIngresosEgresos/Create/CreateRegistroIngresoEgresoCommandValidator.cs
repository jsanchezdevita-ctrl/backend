using FluentValidation;

namespace Application.RegistrosIngresosEgresos.Create;

internal sealed class CreateRegistroIngresoEgresoCommandValidator
    : AbstractValidator<CreateRegistroIngresoEgresoCommand>
{
    public CreateRegistroIngresoEgresoCommandValidator()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario es requerido");

        RuleFor(c => c.EstadoRegistroId)
            .NotEmpty().WithMessage("El estado del registro es requerido");

        RuleFor(c => c)
            .Must(c => c.PuntoEntradaId.HasValue || c.PuntoSalidaId.HasValue)
            .WithMessage("Debe proporcionar al menos un punto de entrada o salida");
    }
}