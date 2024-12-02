﻿using Domain.Errors;
using FluentValidation;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(command => command.SubmittedBy)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidUserId)
                .WithMessage(LeaveRequestErrorMessages.UserIdShouldNotBeNull);

        RuleFor(command => command.LeaveTypeId)
            .NotNull()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidLeaveTypeId)
                .WithMessage(LeaveRequestErrorMessages.LeaveTypeIdShouldNotBeNull);

        RuleFor(command => command.StartDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidStartDate)
                .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeNullOrEmpty);

        RuleFor(command => command.EndDate)
            .NotEmpty()
                .WithErrorCode(LeaveRequestErrorCodes.InvalidEndDate)
                .WithMessage(LeaveRequestErrorMessages.EndDateShouldNotBeNullOrEmpty);

        //TODO: check if user exists or not.

        //TODO: validate if leave request is duplicated
    }
}
