using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class LeaveRequest
{
    public LeaveRequestId Id { get; private set; } = default!;
    public EmployeeId EmployeeId { get; private set; } = default!;
    public LeaveTypeId LeaveTypeId { get; private set; } = default!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveRequestStatus Status { get; private set; } = default!;
    public DateTime RequestDate { get; private set; }
    public string? Comment { get; private set; }
    public EmployeeId ApprovedBy { get; private set; } = null!;

    public LeaveRequest() { } // For EF Core

    public LeaveRequest(
        EmployeeId employeeId,
        LeaveTypeId leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        string? comment = null)
    {
        if (startDate <= DateTime.UtcNow)
            throw new ArgumentException(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);

        if (endDate <= startDate)
            throw new ArgumentException(LeaveRequestErrorMessages.EndDateShouldBeGraterThanStartDate);

        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        StartDate = startDate;
        EndDate = endDate;
        Status = LeaveRequestStatus.Pending;
        RequestDate = DateTime.UtcNow;
        Comment = comment;
    }

    public void Approve(EmployeeId approvedBy)
    {
        Status = LeaveRequestStatus.Approved;
        ApprovedBy = approvedBy;
    }

    public void Reject(EmployeeId approvedBy)
    {
        Status = LeaveRequestStatus.Rejected;
        ApprovedBy = approvedBy;
    }
}
