using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório");
        }
    }
}
