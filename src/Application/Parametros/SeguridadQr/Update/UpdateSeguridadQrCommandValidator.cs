using FluentValidation;

namespace Application.Parametros.SeguridadQr.Update;

internal sealed class UpdateSeguridadQrCommandValidator
    : AbstractValidator<UpdateSeguridadQrCommand>
{
    private readonly string[] _nivelesCifradoValidos = { "AES-128", "AES-192", "AES-256" };

    public UpdateSeguridadQrCommandValidator()
    {
        RuleFor(c => c.VigenciaQRHoras)
            .GreaterThan(0).WithMessage("La vigencia debe ser mayor a 0 horas")
            .LessThanOrEqualTo(720).WithMessage("La vigencia no puede exceder 30 días (720 horas)");

        RuleFor(c => c.IntervaloRenovacionMinutos)
            .GreaterThan(0).WithMessage("El intervalo de renovación debe ser mayor a 0 minutos")
            .LessThanOrEqualTo(1440).WithMessage("El intervalo no puede exceder 24 horas (1440 minutos)");

        RuleFor(c => c.IntervaloRenovacionMinutos)
            .LessThanOrEqualTo(c => c.VigenciaQRHoras * 60)
            .WithMessage("El intervalo de renovación no puede ser mayor que la vigencia total en minutos");

        RuleFor(c => c.NivelCifrado)
            .NotEmpty().WithMessage("El nivel de cifrado es requerido")
            .Must(n => _nivelesCifradoValidos.Contains(n))
            .WithMessage($"Nivel de cifrado debe ser: {string.Join(", ", _nivelesCifradoValidos)}");
    }
}