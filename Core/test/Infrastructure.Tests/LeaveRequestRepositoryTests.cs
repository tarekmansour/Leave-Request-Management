using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Repositories;
using SharedKernel.Tests;

namespace Infrastructure.Tests;
public class LeaveRequestRepositoryTests : DatabaseFixture
{
    private readonly ILeaveRequestRepository _sut;

    public LeaveRequestRepositoryTests() => _sut = new LeaveRequestRepository(_dbContext);

    [Fact(DisplayName = "New LeaveRequestRepository without ApplicationDbContext")]
    public void CreateWithNullApplicationDbContext_Should_ThrowsArgumentException()
    {
        // Arrange
        Func<LeaveRequestRepository> act = () => new LeaveRequestRepository(null!);

        // Act & Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Create new LeaveRequest returns successful result")]
    public async Task CreateLeaveRequestAsync_Should_ReturnsSuccessfulResult()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            id: new LeaveRequestId(1),
            employeeId: new EmployeeId(2),
            leaveTypeId: new LeaveTypeId(1),
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(5),
            comment: "new holidays");

        // Act
        var createdLeaveRequestId = await _sut.CreateLeaveRequestAsync(leaveRequest);

        // Assert
        createdLeaveRequestId.Should().NotBeNull();
        createdLeaveRequestId.Value.Should().Be(1);
    }
}
