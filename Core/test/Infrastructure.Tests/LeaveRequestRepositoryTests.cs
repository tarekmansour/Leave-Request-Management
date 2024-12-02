using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Repositories;
using SharedKernel.Tests;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
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

    [Fact(DisplayName = "Get leave request by Id returns existing record")]
    public async Task GetLeaveRequestByIdAsync_ShouldReturnLeaveRequest_WhenExists()
    {
        // Arrange
        var leaveRequestId = new LeaveRequestId(15);

        var leaveRequest = new LeaveRequest(
            id: leaveRequestId,
            submittedBy: new EmployeeId(1),
            leaveTypeId: new LeaveTypeId(4),
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(5),
            comment: "paternity days off");

        await _dbContext.LeaveRequests.AddAsync(leaveRequest);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetLeaveRequestByIdAsync(leaveRequestId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(leaveRequest);
    }

    [Fact(DisplayName = "Get leave request by Id returns null for none existing record")]
    public async Task GetLeaveRequestByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        var leaveRequestId = new LeaveRequestId(2);

        // Act
        var result = await _sut.GetLeaveRequestByIdAsync(leaveRequestId);

        // Assert
        result.Should().BeNull();
    }
}
