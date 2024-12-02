using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class LeaveRequest
{
    public LeaveRequestId Id { get; private set; } = default!;
    public EmployeeId SubmittedBy { get; private set; } = default!;
    public LeaveTypeId LeaveTypeId { get; private set; } = default!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveRequestStatus Status { get; private set; } = default!;
    public DateTime RequestDate { get; private set; }
    public string? Comment { get; private set; }
    public EmployeeId DecidedBy { get; private set; } = null!;
    public string? DecisionReason { get; private set; }

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

        SubmittedBy = employeeId;
        LeaveTypeId = leaveTypeId;
        StartDate = startDate;
        EndDate = endDate;
        Status = LeaveRequestStatus.Pending;
        RequestDate = DateTime.UtcNow;
        Comment = comment;
    }

    public void Approve(EmployeeId approvedBy, string? decisionReason = null)
    {
        SetDecision(LeaveRequestStatus.Approved, approvedBy, decisionReason);
    }

    public void Reject(EmployeeId rejectedBy, string? decisionReason = null)
    {
        SetDecision(LeaveRequestStatus.Rejected, rejectedBy, decisionReason);
    }

    private void SetDecision(LeaveRequestStatus status, EmployeeId decidedBy, string? decisionReason)
    {
        Status = status;
        DecidedBy = decidedBy;
        DecisionReason = decisionReason;
    }
}
