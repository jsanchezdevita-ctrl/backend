using FluentValidation;

namespace Application.Dispositivos.UpdateConfiguracion;

internal sealed class UpdateDispositivoConfiguracionCommandValidator : AbstractValidator<UpdateDispositivoConfiguracionCommand>
{
    public UpdateDispositivoConfiguracionCommandValidator()
    {
        RuleFor(c => c.DispositivoId)
            .NotEmpty().WithMessage("El ID del dispositivo es requerido");

        RuleFor(c => c.PotenciaTransmision)
            .IsInEnum().WithMessage("La potencia de transmisión es inválida");

    }
}