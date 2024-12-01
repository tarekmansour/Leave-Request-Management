using Domain.Errors;
using FluentValidation;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEmployeeId)
                .WithMessage(LeaveRequestErrorMessages.EmployeeIdShouldNotBeNull);

        RuleFor(command => command.LeaveTypeId)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveTypeId)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeIdShouldNotBeNull);

        RuleFor(command => command.StartDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidStartDate)
                .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeNull);

        RuleFor(command => command.EndDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEndDate)
                .WithMessage(LeaveRequestErrorMessages.EndDateShouldNotBeNull);

        //TODO: validate balance leave according to max days per user
    }
}
