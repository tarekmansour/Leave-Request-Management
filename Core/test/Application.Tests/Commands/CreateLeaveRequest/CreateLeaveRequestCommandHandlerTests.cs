using Application.Commands.CreateLeaveRequest;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

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
            _unitOfWork);

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
            _clientRepository,
            _unitOfWork);

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
            _clientRepository,
            null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        var command = new CreateLeaveRequestCommand(
            EmployeeId: new EmployeeId(2),
            LeaveTypeId: new LeaveTypeId(1),
            StartDate: DateTime.UtcNow.AddDays(7),
            EndDate: DateTime.UtcNow.AddDays(10));

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
