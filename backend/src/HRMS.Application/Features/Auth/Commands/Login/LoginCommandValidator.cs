namespace HRMS.Application.Features.Auth.Commands.Login;

using FluentValidation;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email e obrigatorio.")
            .EmailAddress().WithMessage("Email invalido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha e obrigatoria.")
            .MinimumLength(6).WithMessage("Senha deve ter no minimo 6 caracteres.");
    }
}
