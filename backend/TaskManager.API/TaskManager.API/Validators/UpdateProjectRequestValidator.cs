using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators;

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do projeto é obrigatório")
            .MaximumLength(250).WithMessage("O projeto deve ter no máximo 250 caracteres");
    }
}
