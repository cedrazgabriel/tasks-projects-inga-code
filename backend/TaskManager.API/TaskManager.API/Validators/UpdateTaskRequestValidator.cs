using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O campo 'Name' é obrigatório.")
            .MaximumLength(250).WithMessage("O campo 'Name' não pode ter mais de 250 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("O campo 'Description' é obrigatório.");


        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("O campo projeto id é obrigatório.");

    }
}
