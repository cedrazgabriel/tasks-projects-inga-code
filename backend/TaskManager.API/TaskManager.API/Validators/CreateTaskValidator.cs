using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O campo 'Name' é obrigatório.")
                .MaximumLength(250).WithMessage("O campo 'Name' não pode ter mais de 250 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("O campo 'Description' é obrigatório.");

        }
    }
}
