using Domain.Errors;
using FluentValidation;

namespace Application.Commands.UserLogin;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
                .WithErrorCode(UserErrorCodes.InvalidUserEmail)
                .WithMessage(UserErrorMessages.UserEmailShouldNotBeNullOrEmpty);

        RuleFor(command => command.Password)
            .NotEmpty()
                .WithErrorCode(UserErrorCodes.InvalidUserPassword)
                .WithMessage(UserErrorMessages.UserPasswordShouldNotBeNullOrEmpty);
    }
}
