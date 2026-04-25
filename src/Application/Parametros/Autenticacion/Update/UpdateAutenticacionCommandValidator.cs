using FluentValidation;

namespace Application.Parametros.Autenticacion.Update;

internal sealed class UpdateAutenticacionCommandValidator
    : AbstractValidator<UpdateAutenticacionCommand>
{
    public UpdateAutenticacionCommandValidator()
    {
        RuleFor(c => c.TiempoSesionMinutos)
            .GreaterThan(0).WithMessage("El tiempo de sesión debe ser mayor a 0 minutos")
            .LessThanOrEqualTo(1440).WithMessage("El tiempo de sesión no puede exceder 24 horas (1440 minutos)");

        RuleFor(c => c.IntentosMaximosLogin)
            .GreaterThan(0).WithMessage("Los intentos máximos deben ser mayor a 0")
            .LessThanOrEqualTo(10).WithMessage("Los intentos máximos no pueden exceder 10");
    }
}