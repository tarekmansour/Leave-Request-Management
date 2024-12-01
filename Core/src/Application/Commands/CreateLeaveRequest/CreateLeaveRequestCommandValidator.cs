using Domain.Errors;
using FluentValidation;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEmployeeId)
                .WithMessage(LeaveRequestErrorMessages.EmployeeIdShouldNotBeNull);

        RuleFor(command => command.LeaveTypeId)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveTypeId)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeIdShouldNotBeNull);

        RuleFor(command => command.StartDate)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidStartDate)
                .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeNull);

        RuleFor(command => command.EndDate)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEndDate)
                .WithMessage(LeaveRequestErrorMessages.EndDateShouldNotBeNull);
    }
}
