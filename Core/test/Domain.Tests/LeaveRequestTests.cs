using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Errors;
using Domain.Exceptions;
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

    [Fact(DisplayName = "Update leave type when new leave type is different")]
    public void UpdateLeaveType_ShouldUpdateLeaveType_WhenNewLeaveTypeIsDifferent()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(3),
            endDate: DateTime.UtcNow.AddDays(5));

        var newLeaveType = LeaveType.SickLeave;

        // Act
        leaveRequest.UpdateLeaveType(newLeaveType);

        // Assert
        leaveRequest.LeaveType.Should().Be(newLeaveType);
    }

    [Fact(DisplayName = "Throw exception when new leave type is the same")]
    public void UpdateLeaveType_ShouldThrowException_WhenNewLeaveTypeIsTheSame()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(3),
            endDate: DateTime.UtcNow.AddDays(5));

        var newLeaveType = LeaveType.Off;

        // Act
        Action act = () => leaveRequest.UpdateLeaveType(newLeaveType);

        // Assert
        act.Should().Throw<LeaveRequestException>()
            .WithMessage(LeaveRequestErrorMessages.InvalidNewLeaveType);
    }

    [Fact(DisplayName = "Update start date when new start date is valid")]
    public void UpdateStartDate_ShouldUpdateStartDate_WhenNewStartDateIsValid()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(6));

        var newStartDate = DateTime.UtcNow.AddDays(3);

        // Act
        leaveRequest.UpdateStartDate(newStartDate);

        // Assert
        leaveRequest.StartDate.Should().Be(newStartDate);
    }

    [Fact(DisplayName = "Throw exception when new start date is in the past")]
    public void UpdateStartDate_ShouldThrowException_WhenNewStartDateIsInThePast()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(2),
            endDate: DateTime.UtcNow.AddDays(5));

        var newStartDate = DateTime.UtcNow.AddDays(-1);

        // Act
        Action act = () => leaveRequest.UpdateStartDate(newStartDate);

        // Assert
        act.Should().Throw<LeaveRequestException>()
            .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);
    }

    [Fact(DisplayName = "Throw exception when new start date is after end date")]
    public void UpdateStartDate_ShouldThrowException_WhenNewStartDateIsAfterEndDate()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(2));

        var newStartDate = DateTime.UtcNow.AddDays(3);

        // Act
        Action act = () => leaveRequest.UpdateStartDate(newStartDate);

        // Assert
        act.Should().Throw<LeaveRequestException>()
            .WithMessage(LeaveRequestErrorMessages.StartDateShouldBeBeforeEndDate);
    }

    [Fact(DisplayName = "Update end date when new end date is valid")]
    public void UpdateEndDate_ShouldUpdateEndDate_WhenNewEndDateIsValid()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            submittedBy: new UserId(1),
            leaveType: LeaveType.Off,
            startDate: DateTime.UtcNow.AddDays(1),
            endDate: DateTime.UtcNow.AddDays(3));

        var newEndDate = DateTime.UtcNow.AddDays(4);

        // Act
        leaveRequest.UpdateEndDate(newEndDate);

        // Assert
        leaveRequest.EndDate.Should().Be(newEndDate);
    }

    [Fact(DisplayName = "Throw exception when new end date is before or equal to start date")]
    public void UpdateEndDate_ShouldThrowException_WhenNewEndDateIsBeforeOrEqualToStartDate()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
             submittedBy: new UserId(1),
             leaveType: LeaveType.Off,
             startDate: DateTime.UtcNow.AddDays(2),
             endDate: DateTime.UtcNow.AddDays(3));

        var newEndDate = DateTime.UtcNow.AddDays(1);

        // Act
        Action act = () => leaveRequest.UpdateEndDate(newEndDate);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage(LeaveRequestErrorMessages.EndDateShouldBeAfterStartDate);
    }

    [Fact(DisplayName = "Throw exception when reject request without decision reason")]
    public void UpdateEndDate_ShouldThrowException_WhenRejectedRequestWithoutReasonProvided()
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
        act.Should().Throw<LeaveRequestException>()
            .WithMessage(LeaveRequestErrorMessages.ForRejectedRequestsReasonShouldBeProvided);
    }

    [Fact(DisplayName = "should update status when decided by a valid user")]
    public void UpdateStatus_ShouldUpdateStatus_WhenDecidedByValidUser()
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
