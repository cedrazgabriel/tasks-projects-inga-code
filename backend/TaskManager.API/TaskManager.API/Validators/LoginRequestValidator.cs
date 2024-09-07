
using FluentValidation;
using TaskManager.API.DTO.Request;

namespace TaskManager.API.Validators
{
    public class LoginRequestValidator:  AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .MaximumLength(250).WithMessage("O nome de usuário deve ter no máximo 250 caracteres"); ;

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MaximumLength(512).WithMessage("A senha deve ter no máximo 512 caracteres"); ;
        }
    }
}
