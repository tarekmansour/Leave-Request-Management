using Application.Commands.UpdateLeaveRequestStatus;
using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

namespace Application.Tests.Commands.UpdateLeaveRequestStatus;
public partial class UpdateLeaveRequestStatusCommandTests
{
    [Fact(DisplayName = "UpdateLeaveRequestStatusCommand with invalid LeaveRequestId")]
    public async Task WithInvalidLeaveRequestId_Should_ReturnsError()
    {
        //Arrange
        var command = new UpdateLeaveRequestStatusCommand(
            LeaveRequestId: null!,
            NewStatus: LeaveRequestStatus.Rejected);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidLeaveRequestId);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.LeaveRequestIdShouldNotBeNull);
    }

    [Fact(DisplayName = "UpdateLeaveRequestStatusCommand with invalid LeaveRequestStatus")]
    public async Task WithInvalidLeaveRequestStatus_Should_ReturnsError()
    {
        //Arrange
        var command = new UpdateLeaveRequestStatusCommand(
            LeaveRequestId: new LeaveRequestId(1),
            NewStatus: null!);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidLeaveRequestStatus);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.LeaveRequestStatusShouldNotBeNullOrEmpty);
    }
}
