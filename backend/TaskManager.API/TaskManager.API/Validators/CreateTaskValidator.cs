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
                .MaximumLength(100).WithMessage("O campo 'Name' não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("O campo 'Description' é obrigatório.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("O campo 'ProjectId' é obrigatório.")
                .Must(projectId => Guid.TryParse(projectId.ToString(), out _))
                .WithMessage("O campo 'ProjectId' precisa ser um GUID válido.");
        }
    }
}
