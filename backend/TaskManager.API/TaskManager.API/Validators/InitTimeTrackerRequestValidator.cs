using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators;

public class InitTimeTrackerRequestValidator : AbstractValidator<InitTimeTrackerRequest>
{
    public InitTimeTrackerRequestValidator()
    {
           
        RuleFor(x => x.TimeZoneId)
            .NotEmpty().WithMessage("O time zone id é obrigatório.");

        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("O campo task id é obrigatório.");

    }
}
