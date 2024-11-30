using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;

namespace Domain.Tests;
public class LeaveRequestTests
{
    [Fact(DisplayName = "New leave request always with pending status")]
    public void NewLeaveRequest_ShouldBeOn_PendingStatus()
    {
        // Arrange
        var employeeId = new EmployeeId(1);
        var leaveTypeId = new LeaveTypeId(2);
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(5);

        // Act
        var newLeaveRequest = new LeaveRequest(
            employeeId: employeeId,
            leaveTypeId: leaveTypeId,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Status.Should().Be(LeaveRequestStatus.Pending);
    }

    [Fact(DisplayName = "New Leave request with start date greater than the end date")]
    public void NewLeaveRequest_ShouldValidate_EntryDates()
    {
        // Arrange
        var employeeId = new EmployeeId(10);
        var leaveTypeId = new LeaveTypeId(3);
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = DateTime.UtcNow.AddDays(2);

        // Act
        Func<LeaveRequest> newLeaveRequest = () => new LeaveRequest(
            employeeId: employeeId,
            leaveTypeId: leaveTypeId,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Should().Throw<ArgumentException>()
            .WithMessage(LeaveRequestErrorMessages.EndDateShouldBeGraterThanStartDate);
    }

    [Fact(DisplayName = "New Leave request with start date in the past")]
    public void NewLeaveRequest_ShouldValidate_StartDate()
    {
        // Arrange
        var employeeId = new EmployeeId(10);
        var leaveTypeId = new LeaveTypeId(3);
        var startDate = DateTime.UtcNow.AddDays(-2);
        var endDate = DateTime.UtcNow.AddDays(5);

        // Act
        Func<LeaveRequest> newLeaveRequest = () => new LeaveRequest(
            employeeId: employeeId,
            leaveTypeId: leaveTypeId,
            startDate: startDate,
            endDate: endDate);

        // Assert
        newLeaveRequest.Should().Throw<ArgumentException>()
            .WithMessage(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);
    }
}
