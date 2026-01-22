using FluentValidation;

namespace core.application.Features.Clientes.Commands.DeleteClienteCommand
{
    public class DeleteClientCommandValidator : AbstractValidator<DeleteClienteCommand>
    {
        public DeleteClientCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("El {PropertyName} del cliente no puede ser nulo.")
                .GreaterThan(0).WithMessage("El {PropertyName} del cliente debe ser mayor que 0.");
        }
    }
}
