using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Commands.UpdateLeaveRequest;
public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    public UpdateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.LeaveRequestId)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveRequestId)
                .WithMessage(LeaveRequestErrorMessages.LeaveRequestIdShouldNotBeNull);

        RuleFor(command => command.LeaveType)
            .Must((command, leaveType) =>
            {
                return LeaveType.IsValidLeaveType(leaveType);
            })
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveType)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeNotSupported)
            .When(command => command.LeaveType is not null);

        RuleFor(command => command.NewStatus)
            .Must((command, newStatus) =>
            {
                return LeaveRequestStatus.IsValidLeaveRequestStatus(newStatus);
            })
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveRequestStatus)
                .WithMessage(LeaveRequestErrorMessages.LeaveRequestStatusNotSupported)
            .When(command => command.NewStatus is not null);
    }
}