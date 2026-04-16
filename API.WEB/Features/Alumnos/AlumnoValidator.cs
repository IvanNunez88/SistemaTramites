using FluentValidation;
using API.WEB.Features.Alumnos.Dominio.Dto;

namespace API.WEB.Features.Alumnos;

public class CrearAlumnoValidator : AbstractValidator<CrearAlumnoDTO>
{
    public CrearAlumnoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(80).WithMessage("El nombre no puede tener mas de 80 caracteres");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MaximumLength(120).WithMessage("Los apellidos no pueden tener mas de 120 caracteres");

        RuleFor(x => x.Tel)
            .NotEmpty().WithMessage("El telefono es requerido")
            .MaximumLength(15).WithMessage("El telefono no puede tener mas de 15 caracteres");

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage("El correo es requerido")
            .EmailAddress().WithMessage("El correo no es valido")
            .MaximumLength(80).WithMessage("El correo no puede tener mas de 80 caracteres");
    }
}

public class ActualizarAlumnoValidator : AbstractValidator<ActualizarAlumnoDTO>
{
    public ActualizarAlumnoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(80).WithMessage("El nombre no puede tener mas de 80 caracteres");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MaximumLength(120).WithMessage("Los apellidos no pueden tener mas de 120 caracteres");

        RuleFor(x => x.Tel)
            .NotEmpty().WithMessage("El telefono es requerido")
            .MaximumLength(15).WithMessage("El telefono no puede tener mas de 15 caracteres");

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage("El correo es requerido")
            .EmailAddress().WithMessage("El correo no es valido")
            .MaximumLength(80).WithMessage("El correo no puede tener mas de 80 caracteres");
    }
}
