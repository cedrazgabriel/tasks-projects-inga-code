
using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators
{
    public class LoginRequestValidator:  AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória");
        }
    }
}
