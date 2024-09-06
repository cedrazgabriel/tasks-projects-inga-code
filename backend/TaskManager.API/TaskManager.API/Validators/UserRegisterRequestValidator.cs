using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");
        }
    }
}
