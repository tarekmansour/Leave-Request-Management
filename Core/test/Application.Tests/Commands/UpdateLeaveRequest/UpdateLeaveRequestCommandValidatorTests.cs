﻿using Application.Commands.UpdateLeaveRequest;
using Domain.Errors;
using FluentAssertions;

namespace Application.Tests.Commands.UpdateLeaveRequest;
public partial class UpdateLeaveRequestCommandTests
{
    [Fact(DisplayName = "UpdateLeaveRequestCommand with invalid LeaveType")]
    public async Task WithInvalidLeaveRequestId_Should_ReturnsError()
    {
        //Arrange
        var command = new UpdateLeaveRequestCommand(
            LeaveRequestId: 1,
            LeaveType: "Vacation");

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidLeaveType);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.LeaveTypeNotSupported);
    }

    [Fact(DisplayName = "UpdateLeaveRequestCommand with invalid new status")]
    public async Task WithInvalidLeaveRequest_Should_ReturnsError()
    {
        //Arrange
        var command = new UpdateLeaveRequestCommand(
            LeaveRequestId: 1,
            NewStatus: "done");

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidLeaveRequestStatus);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.LeaveRequestStatusNotSupported);
    }
}