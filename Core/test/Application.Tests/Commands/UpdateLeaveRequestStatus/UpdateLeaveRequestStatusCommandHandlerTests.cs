using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using NSubstitute;

namespace Application.Tests.Commands.UpdateLeaveRequestStatus;
public partial class UpdateLeaveRequestStatusCommandTests
{
    [Fact(DisplayName = "new UpdateLeaveRequestStatusCommandHandler with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new UpdateLeaveRequestStatusCommandHandler(
            _logger,
            _validator,
            null!,
            _unitOfWork);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new UpdateLeaveRequestStatusCommandHandler with null logger")]
    public void WithNullLogger_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new UpdateLeaveRequestStatusCommandHandler(
            null!,
            _validator,
            _leaveRequestRepository,
            _unitOfWork);

        // Assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "new UpdateLeaveRequestStatusCommandHandler with null unitOfWork")]
    public void WithNullUnitOfWork_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new UpdateLeaveRequestStatusCommandHandler(
            _logger,
            _validator,
            _leaveRequestRepository,
            null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        var expectedLeaveRequest = new LeaveRequest(
            submittedBy: new UserId(3),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(20),
            endDate: DateTime.UtcNow.AddDays(30),
            comment: "validate with my team.");

        var command = new UpdateLeaveRequestStatusCommand(
            LeaveRequestId: new LeaveRequestId(1),
            NewStatus: LeaveRequestStatus.Approved);

        _leaveRequestRepository.GetByIdAsync(Arg.Any<LeaveRequestId>())
            .Returns(expectedLeaveRequest);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(LeaveRequestStatus.Approved.ToString());
    }

    [Fact(DisplayName = "Handle returns failure")]
    public async Task Handle_Should_ReturnsFailure()
    {
        //Arrange
        var command = new UpdateLeaveRequestStatusCommand(
            LeaveRequestId: new LeaveRequestId(1),
            NewStatus: LeaveRequestStatus.Approved);

        _leaveRequestRepository.GetByIdAsync(Arg.Any<LeaveRequestId>())
            .Returns(Task.FromResult<LeaveRequest?>(null));


        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
    }

}
