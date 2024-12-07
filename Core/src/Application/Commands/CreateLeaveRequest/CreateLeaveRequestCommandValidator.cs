using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.LeaveType)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveType)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeShouldNotBeNullOrEmpty)
            .Must((command, leaveType) =>
            {
                return LeaveType.IsValidLeaveType(leaveType);
            })
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveType)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeNotSupported);

        RuleFor(command => command.StartDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidStartDate)
                .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeNullOrEmpty);

        RuleFor(command => command.EndDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEndDate)
                .WithMessage(LeaveRequestErrorMessages.EndDateShouldNotBeNullOrEmpty);
    }
}
