using FluentValidation;

namespace Application.Zonas.Create;

internal sealed class CreateZonaCommandValidator : AbstractValidator<CreateZonaCommand>
{
    public CreateZonaCommandValidator()
    {
        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(c => c.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(c => !string.IsNullOrEmpty(c.Descripcion));

        RuleFor(c => c.Roles)
            .NotEmpty().WithMessage("Debe asignar al menos un rol a la zona")
            .ForEach(rol => rol.ChildRules(rolValidator =>
            {
                rolValidator.RuleFor(r => r.RolId)
                    .NotEmpty().WithMessage("El ID del rol es requerido");

                rolValidator.RuleFor(r => r.CapacidadMaxima)
                    .GreaterThan(0).WithMessage("La capacidad máxima debe ser mayor a cero");
            }));

        RuleFor(c => c.PuntoControlIds)
            .NotEmpty().WithMessage("Debe asignar al menos un punto de control a la zona")
            .ForEach(pcId => pcId.NotEmpty().WithMessage("El ID del punto de control es requerido"));
    }
}