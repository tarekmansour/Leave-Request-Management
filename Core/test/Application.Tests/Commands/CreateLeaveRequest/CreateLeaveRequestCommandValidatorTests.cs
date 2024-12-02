using Application.Commands.CreateLeaveRequest;
using Domain.Errors;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

namespace Application.Tests.Commands.CreateLeaveRequest;

public partial class CreateLeaveRequestCommandTests
{
    [Fact(DisplayName = "CreateLeaveRequestCommand with invalid properties")]
    public async Task WithInvalidProps_Should_ReturnsError()
    {
        //Arrange
        var command = new CreateLeaveRequestCommand(
            EmployeeId: null!,
            LeaveTypeId: new LeaveTypeId(3),
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(5));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault()!.Code.Should().Be(LeaveRequestErrorCodes.InvalidEmployeeId);
        result.Errors.FirstOrDefault()!.Description.Should().Be(LeaveRequestErrorMessages.EmployeeIdShouldNotBeNull);
    }
}
