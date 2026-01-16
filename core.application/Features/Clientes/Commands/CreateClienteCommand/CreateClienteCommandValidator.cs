using FluentValidation;

namespace core.application.Features.Clientes.Commands.CreateClienteCommand
{
    public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteCommandValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacío.")
                .MaximumLength(100).WithMessage("{PropertyName} no puede exceder {MaxLength} caracteres.");

            RuleFor(c => c.Apellido)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacío.")
                .MaximumLength(100).WithMessage("{PropertyName} no puede exceder {MaxLength} caracteres.");

            RuleFor(c => c.FechaNacimiento)
                .NotEmpty().WithMessage("Fecha de nacimiento no puede ser vacía.")
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser una fecha pasada.");

            RuleFor(c => c.Telefono)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacío.")
                .MaximumLength(10).WithMessage("{PropertyName} no puede exceder {MaxLength} caracteres.")
                .Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("{PropertyName} debe tener el formato XXX-XXX-XXXX.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacío.")
                .MaximumLength(100).WithMessage("{PropertyName} no puede exceder {MaxLength} caracteres.")
                .EmailAddress().WithMessage("{PropertyName} debe ser una dirección de correo electrónico válida.");

            RuleFor(c => c.Direccion)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacía.")
                .MaximumLength(200).WithMessage("{PropertyName} no puede exceder {MaxLength} caracteres.");
        }
    }
}
