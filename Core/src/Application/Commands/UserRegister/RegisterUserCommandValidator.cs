using Domain.Errors;
using FluentValidation;

namespace Application.Commands.UserRegister;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
                .WithErrorCode(UserErrorCodes.InvalidUserEmail)
                .WithMessage(UserErrorMessages.UserEmailShouldNotBeNullOrEmpty);

        RuleFor(command => command.FirstName)
            .NotEmpty()
                .WithErrorCode(UserErrorCodes.InvalidUserFirstName)
                .WithMessage(UserErrorMessages.UserFirstNameShouldNotBeNullOrEmpty);

        RuleFor(command => command.LastName)
            .NotEmpty()
                .WithErrorCode(UserErrorCodes.InvalidUserLastName)
                .WithMessage(UserErrorMessages.UserLastNameShouldNotBeNullOrEmpty);

        RuleFor(command => command.Password)
            .NotEmpty()
                .WithErrorCode(UserErrorCodes.InvalidUserPassword)
                .WithMessage(UserErrorMessages.UserPasswordShouldNotBeNullOrEmpty)
            .MinimumLength(8)
                .WithErrorCode(UserErrorCodes.InvalidPasswordLength)
                .WithMessage(UserErrorMessages.PasswordLengthShouldHaveMinimumDigits);
    }
}