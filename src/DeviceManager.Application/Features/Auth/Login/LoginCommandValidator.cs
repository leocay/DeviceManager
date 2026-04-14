using FluentValidation;

namespace DeviceManager.Application.Features.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(200);
    }
}