using Application.Commands.CreateLeaveRequest;
using FluentAssertions;
using NSubstitute;

namespace Application.Tests.Commands.CreateLeaveRequest;
public partial class CreateLeaveRequestCommandTests
{
    [Fact(DisplayName = "new CreateLeaveRequestCommandHandler with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new CreateLeaveRequestCommandHandler(
            _logger,
            _validator,
            null!,
            _unitOfWork,
            _userContext);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new CreateLeaveRequestCommandHandler with null logger")]
    public void WithNullLogger_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new CreateLeaveRequestCommandHandler(
            null!,
            _validator,
            _leaveRequestRepository,
            _unitOfWork,
            _userContext);

        // Assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "new CreateLeaveRequestCommandHandler with null unitOfWork")]
    public void WithNullUnitOfWork_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new CreateLeaveRequestCommandHandler(
            _logger,
            _validator,
            _leaveRequestRepository,
            null!,
            _userContext);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        const int mockUserId = 1;
        _userContext.UserId.Returns(mockUserId);

        var command = new CreateLeaveRequestCommand(
            LeaveType: "off",
            StartDate: DateTime.UtcNow.AddDays(7),
            EndDate: DateTime.UtcNow.AddDays(10));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
