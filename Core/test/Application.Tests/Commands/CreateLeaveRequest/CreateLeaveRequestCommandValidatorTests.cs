using Application.Commands.CreateLeaveRequest;
using Domain.Errors;
using FluentAssertions;

namespace Application.Tests.Commands.CreateLeaveRequest;

public partial class CreateLeaveRequestCommandTests
{
    [Fact(DisplayName = "CreateLeaveRequestCommand with invalid leave type")]
    public async Task WithInvalidProps_Should_ReturnsError()
    {
        //Arrange
        var command = new CreateLeaveRequestCommand(
            LeaveType: "invalidLeaveType",
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(5));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidLeaveType);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.LeaveTypeNotSupported);
    }
}
