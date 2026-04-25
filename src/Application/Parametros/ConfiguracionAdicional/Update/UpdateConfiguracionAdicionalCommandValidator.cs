using FluentValidation;

namespace Application.Parametros.ConfiguracionAdicional.Update;

internal sealed class UpdateConfiguracionAdicionalCommandValidator
    : AbstractValidator<UpdateConfiguracionAdicionalCommand>
{
    private readonly string[] _frecuenciasValidas = { "Diario", "Semanal", "Mensual" };

    public UpdateConfiguracionAdicionalCommandValidator()
    {
        RuleFor(c => c.ZonaHoraria)
            .NotEmpty().WithMessage("La zona horaria es requerida")
            .Must(BeValidTimeZone).WithMessage("Zona horaria no válida");

        RuleFor(c => c.RetencionLogsDias)
            .GreaterThan(0).WithMessage("La retención debe ser mayor a 0 días")
            .LessThanOrEqualTo(365).WithMessage("La retención no puede exceder 365 días");

        RuleFor(c => c.FrecuenciaBackup)
            .NotEmpty().WithMessage("La frecuencia de backup es requerida")
            .Must(f => _frecuenciasValidas.Contains(f))
            .WithMessage($"Frecuencia debe ser: {string.Join(", ", _frecuenciasValidas)}");
    }

    private static bool BeValidTimeZone(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}