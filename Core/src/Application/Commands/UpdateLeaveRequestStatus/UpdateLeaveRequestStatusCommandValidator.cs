using Domain.Errors;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Commands.UpdateLeaveRequestStatus;
public class UpdateLeaveRequestStatusCommandValidator : AbstractValidator<UpdateLeaveRequestStatusCommand>
{
    public UpdateLeaveRequestStatusCommandValidator(ILeaveRequestRepository leaveRequestRepository)
    {
        RuleFor(command => command.LeaveRequestId)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveRequestId)
                .WithMessage(LeaveRequestErrorMessages.LeaveRequestIdShouldNotBeNull);

        RuleFor(command => command.NewStatus)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveRequestStatus)
                .WithMessage(LeaveRequestErrorMessages.LeaveRequestStatusShouldNotBeNullOrEmpty);
    }
}