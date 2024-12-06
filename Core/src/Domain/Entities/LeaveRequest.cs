using Domain.Errors;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class LeaveRequest
{
    public LeaveRequestId Id { get; private set; } = default!;
    public UserId SubmittedBy { get; private set; } = default!;
    public LeaveType LeaveType { get; private set; } = default!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveRequestStatus Status { get; private set; } = default!;
    public string? Comment { get; private set; }
    public UserId DecidedBy { get; private set; } = null!;
    public string? DecisionReason { get; private set; }

    public LeaveRequest() { } // For EF Core

    public LeaveRequest(
        UserId submittedBy,
        LeaveType leaveType,
        DateTime startDate,
        DateTime endDate,
        string? comment = null)
    {
        if (startDate <= DateTime.UtcNow)
            throw new ArgumentException(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);

        if (endDate <= startDate)
            throw new ArgumentException(LeaveRequestErrorMessages.EndDateShouldBeAfterStartDate);

        SubmittedBy = submittedBy;
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        Status = LeaveRequestStatus.Pending;
        Comment = comment;
    }

    public LeaveRequest(
        LeaveRequestId id,
        UserId submittedBy,
        LeaveType leaveType,
        DateTime startDate,
        DateTime endDate,
        string? comment)
    {
        Id = id;
        SubmittedBy = submittedBy;
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        Status = LeaveRequestStatus.Pending;
        Comment = comment;
    }

    public LeaveRequest(
        LeaveRequestId id,
        LeaveType leaveType,
        DateTime startDate,
        DateTime endDate,
        LeaveRequestStatus status)
    {
        Id = id;
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    public LeaveRequest(
        LeaveRequestId id,
        LeaveType leaveType,
        DateTime startDate,
        DateTime endDate,
        LeaveRequestStatus status,
        UserId decidedBy,
        string? decisionReason)
    {
        Id = id;
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        DecidedBy = decidedBy;
        DecisionReason = decisionReason;
    }

    public void UpdateStatus(
        LeaveRequestStatus newStatus,
        UserId decidedBy,
        string? decisionReason)
    {
        if (newStatus != LeaveRequestStatus.Approved && newStatus != LeaveRequestStatus.Rejected)
            throw new InvalidOperationException(LeaveRequestErrorMessages.UnsupportedStatus);

        if (decidedBy == null)
            throw new InvalidOperationException(LeaveRequestErrorMessages.ValidUserShouldApproveLeaveRequest);

        if (newStatus == LeaveRequestStatus.Rejected && decisionReason is null)
            throw new InvalidOperationException(LeaveRequestErrorMessages.ForRejectedRequestsReasonShouldBeProvided);

        Status = newStatus;
        DecidedBy = decidedBy;
        DecisionReason = decisionReason;
    }
}
