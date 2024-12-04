using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

namespace Domain.Tests;

[ExcludeFromCodeCoverage]
public class LeaveRequestTests
{
    [Fact(DisplayName = "New leave request always with pending status")]
    public void NewLeaveRequest_ShouldBeOn_PendingStatus()
    {
        // Arrange
        var submittedBy = new UserId(1);
        var leaveType = LeaveType.Off;
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(5);

        // Act
        var newLeaveRequest = new LeaveRequest(
            submittedBy: submittedBy,
            leaveType: leaveType,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Status.Should().Be(LeaveRequestStatus.Pending);
    }

    [Fact(DisplayName = "New Leave request with start date greater than the end date")]
    public void NewLeaveRequest_ShouldValidate_EntryDates()
    {
        // Arrange
        var submittedBy = new UserId(10);
        var leaveType = LeaveType.SickLeave;
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = DateTime.UtcNow.AddDays(2);

        // Act
        Func<LeaveRequest> newLeaveRequest = () => new LeaveRequest(
            submittedBy: submittedBy,
            leaveType: leaveType,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Should().Throw<ArgumentException>()
            .WithMessage(LeaveRequestErrorMessages.EndDateShouldBeAfterStartDate);
    }

    [Fact(DisplayName = "New Leave request with start date in the past")]
    public void NewLeaveRequest_ShouldValidate_StartDate()
    {
        // Arrange
        var submittedBy = new UserId(10);
        var leaveType = LeaveType.Off;
        var startDate = DateTime.UtcNow.AddDays(-2);
        var endDate = DateTime.UtcNow.AddDays(5);

        // Act
        Func<LeaveRequest> newLeaveRequest = () => new LeaveRequest(
            submittedBy: submittedBy,
            leaveType: leaveType,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Should().Throw<ArgumentException>()
            .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);
    }

    [Fact(DisplayName = "Approve a leave request")]
    public void Approve_Should_UpdateStatus()
    {
        // Arrange
        var createdLeaveRequest = new LeaveRequest(
            submittedBy: new UserId(3),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(10),
            endDate: DateTime.UtcNow.AddDays(20));

        var HRUserId = new UserId(1);

        // Act
        createdLeaveRequest.UpdateProperties(LeaveRequestStatus.Approved, HRUserId);

        // Assert
        createdLeaveRequest.Status.Should().Be(LeaveRequestStatus.Approved);
        createdLeaveRequest.DecidedBy.Should().Be(HRUserId);
        createdLeaveRequest.DecisionReason.Should().BeNull();
    }

    [Fact(DisplayName = "Reject a leave request")]
    public void Reject_Should_UpdateStatus()
    {
        // Arrange
        var createdLeaveRequest = new LeaveRequest(
            submittedBy: new UserId(3),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(10),
            endDate: DateTime.UtcNow.AddDays(20));

        var managerId = new UserId(2);

        // Act
        createdLeaveRequest.UpdateProperties(LeaveRequestStatus.Rejected, managerId, "not valid period");

        // Assert
        createdLeaveRequest.Status.Should().Be(LeaveRequestStatus.Rejected);
        createdLeaveRequest.DecidedBy.Should().Be(managerId);
        createdLeaveRequest.DecisionReason.Should().Be("not valid period");
    }
}
