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
    [Fact(DisplayName = "New leave request always created with pending status")]
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

    [Fact(DisplayName = "New Leave request with end date should be after the start date")]
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

    [Fact(DisplayName = "New Leave request should not be with start date in the past")]
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

    [Fact(DisplayName = "Reject request should be with decision reason")]
    public void RejectLeaveRequest_ShouldBe_WithDecisionReason()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
             submittedBy: new UserId(1),
             leaveType: LeaveType.Off,
             startDate: DateTime.UtcNow.AddDays(2),
             endDate: DateTime.UtcNow.AddDays(3));

        var newStatus = LeaveRequestStatus.Rejected;
        var decidedBy = new UserId(2);

        // Act
        Action act = () => leaveRequest.UpdateStatus(newStatus, decidedBy, null);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage(LeaveRequestErrorMessages.ForRejectedRequestsReasonShouldBeProvided);
    }

    [Fact(DisplayName = "Update status should be decided by a valid user")]
    public void UpdateStatus_ShouldUpdateStatus_ByValidUser()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
             submittedBy: new UserId(1),
             leaveType: LeaveType.SickLeave,
             startDate: DateTime.UtcNow.AddDays(2),
             endDate: DateTime.UtcNow.AddDays(3));

        var newStatus = LeaveRequestStatus.Approved;
        var decidedBy = new UserId(2);
        var decisionReason = "Approved for medical leave";

        // Act
        leaveRequest.UpdateStatus(newStatus, decidedBy, decisionReason);

        // Assert
        leaveRequest.Status.Should().Be(newStatus);
        leaveRequest.DecidedBy.Should().Be(decidedBy);
        leaveRequest.DecisionReason.Should().Be(decisionReason);
    }
}
