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

        RuleFor(command => command.Status)
            .Must((command, newStatus) =>
            {
                return LeaveRequestStatus.IsValidLeaveRequestStatus(newStatus);
            })
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveRequestStatus)
                .WithMessage(LeaveRequestErrorMessages.LeaveRequestStatusNotSupported);
    }
}