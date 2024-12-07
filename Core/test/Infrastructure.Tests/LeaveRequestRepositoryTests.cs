using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
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
            submittedBy: new UserId(2),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(5),
            comment: "new holidays");

        // Act
        var createdLeaveRequestId = await _sut.CreateAsync(leaveRequest);

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
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(5),
            comment: "paternity days off");

        await _dbContext.LeaveRequests.AddAsync(leaveRequest);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(leaveRequestId);

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
        var result = await _sut.GetByIdAsync(leaveRequestId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "List leave requests by user")]
    public async Task GetLeaveRequestsAsync_ShouldReturn_AllRequests()
    {
        // Arrange
        var userId = new UserId(1);
        var HRUserId = new UserId(2);

        var myFirstLeaveRequest = new LeaveRequest(
            id: new LeaveRequestId(1),
            submittedBy: userId,
            leaveType: LeaveType.Paternity,
            startDate: DateTime.UtcNow.AddDays(10),
            endDate: DateTime.UtcNow.AddDays(25),
            comment: "paternity days off");

        var mySecondLeaveRequest = new LeaveRequest(
            id: new LeaveRequestId(2),
            submittedBy: userId,
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(30),
            endDate: DateTime.UtcNow.AddDays(36),
            comment: "just off");

        mySecondLeaveRequest.UpdateStatus(
            newStatus: LeaveRequestStatus.Rejected,
            decidedBy: HRUserId,
            decisionReason: "HR rejection!");

        await _dbContext.LeaveRequests.AddRangeAsync(myFirstLeaveRequest, mySecondLeaveRequest);
        await _dbContext.SaveChangesAsync();


        // Act
        var result = await _sut.GetAllByUserAsync(userId, null);

        // Assert
        result.Count.Should().Be(2);
        result.ElementAt(0)?.LeaveType.Should().Be(LeaveType.Paternity);
        result.ElementAt(0)?.Status.Should().Be(LeaveRequestStatus.Pending);

        result.ElementAt(1)?.LeaveType.Should().Be(LeaveType.Off);
        result.ElementAt(1)?.Status.Should().Be(LeaveRequestStatus.Rejected);
        result.ElementAt(1)?.DecidedBy.Should().Be(HRUserId);
        result.ElementAt(1)?.DecisionReason.Should().Be("HR rejection!");
    }

    [Fact(DisplayName = "List leave requests by user returns empty list")]
    public async Task GetLeaveRequestsAsync_ShouldReturn_EmptyList()
    {
        // Arrange & Act
        var result = await _sut.GetAllByUserAsync(new UserId(1), null);

        // Assert
        result.Count.Should().Be(0);
    }

    [Fact(DisplayName = "List leave requests by user with status filter")]
    public async Task GetLeaveRequestsAsync_ShouldReturn_AllRequestsFilteredByStatus()
    {
        // Arrange
        var userId = new UserId(1);
        var HRUserId = new UserId(2);

        var myFirstLeaveRequest = new LeaveRequest(
            id: new LeaveRequestId(1),
            submittedBy: userId,
            leaveType: LeaveType.Paternity,
            startDate: DateTime.UtcNow.AddDays(10),
            endDate: DateTime.UtcNow.AddDays(25),
            comment: "paternity days off");

        var mySecondLeaveRequest = new LeaveRequest(
            id: new LeaveRequestId(2),
            submittedBy: userId,
            leaveType: LeaveType.SickLeave,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(2),
            comment: "I'm sick");

        myFirstLeaveRequest.UpdateStatus(newStatus: LeaveRequestStatus.Approved, decidedBy: HRUserId, null);

        await _dbContext.LeaveRequests.AddRangeAsync(myFirstLeaveRequest, mySecondLeaveRequest);
        await _dbContext.SaveChangesAsync();


        // Act
        var result = await _sut.GetAllByUserAsync(userId, LeaveRequestStatus.Pending);

        // Assert
        result.Count.Should().Be(1);
        result.FirstOrDefault()?.Status.Should().Be(LeaveRequestStatus.Pending);
        result.FirstOrDefault()?.LeaveType.Should().Be(LeaveType.SickLeave);
    }
}
