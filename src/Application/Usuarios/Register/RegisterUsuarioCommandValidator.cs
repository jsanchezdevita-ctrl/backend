using FluentValidation;

namespace Application.Usuarios.Register;

internal sealed class RegisterUsuarioCommandValidator : AbstractValidator<RegisterUsuarioCommand>
{
    public RegisterUsuarioCommandValidator()
    {
        RuleFor(c => c.NumeroDocumento).NotEmpty();
        RuleFor(c => c.Nombre).NotEmpty();
        RuleFor(c => c.Apellido).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Estado).IsInEnum();
        RuleFor(c => c.HorarioInicio).NotEmpty();
        RuleFor(c => c.HorarioFin).NotEmpty();

        RuleFor(c => c.RolIds)
            .NotEmpty()
            .ForEach(rolId => rolId.NotEmpty());

        RuleFor(c => c.HorarioFin)
            .GreaterThan(c => c.HorarioInicio)
            .WithMessage("El horario de fin debe ser posterior al horario de inicio");

    }
}
