using FluentValidation;
using System.Net;

namespace Application.Dispositivos.Create;

internal sealed class CreateDispositivoCommandValidator : AbstractValidator<CreateDispositivoCommand>
{
    public CreateDispositivoCommandValidator()
    {
        RuleFor(c => c.DispositivoId)
            .NotEmpty().WithMessage("El ID del dispositivo es requerido")
            .MaximumLength(20).WithMessage("El ID del dispositivo no puede exceder 20 caracteres");

        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre del dispositivo es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(c => c.DireccionIp)
            .NotEmpty().WithMessage("La dirección IP es requerida")
            .Must(BeValidIpAddress).WithMessage("La dirección IP tiene un formato inválido");

        RuleFor(c => c.PuntoControlId)
            .NotEmpty().WithMessage("El punto de control es requerido");
    }

    private static bool BeValidIpAddress(string ip)
    {
        return IPAddress.TryParse(ip, out _);
    }
}