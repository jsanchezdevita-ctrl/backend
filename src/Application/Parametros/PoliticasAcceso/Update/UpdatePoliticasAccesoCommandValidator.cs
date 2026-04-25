using FluentValidation;

namespace Application.Parametros.PoliticasAcceso.Update;

internal sealed class UpdatePoliticasAccesoCommandValidator
    : AbstractValidator<UpdatePoliticasAccesoCommand>
{
    public UpdatePoliticasAccesoCommandValidator()
    {
        RuleFor(c => c.TiempoBloqueoSegundos)
            .GreaterThan(0).WithMessage("El tiempo de bloqueo debe ser mayor a 0 segundos")
            .LessThanOrEqualTo(300).WithMessage("El tiempo de bloqueo no puede exceder 5 minutos (300 segundos)");

        RuleFor(c => c)
            .Must(c => !c.BloqueoAutomaticoPuertas || c.TiempoBloqueoSegundos > 0)
            .WithMessage("Si el bloqueo automático está activado, el tiempo de bloqueo debe ser mayor a 0");
    }
}