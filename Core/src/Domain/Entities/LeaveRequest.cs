using Domain.Errors;
using Domain.Exceptions;
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
    public DateTime RequestDate { get; private set; }
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
        RequestDate = DateTime.UtcNow;
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
        RequestDate = DateTime.UtcNow;
        Comment = comment;
    }

    public void UpdateLeaveType(LeaveType newLeaveType)
    {
        if (LeaveType == newLeaveType)
            throw new LeaveRequestException(LeaveRequestErrorMessages.InvalidNewLeaveType);

        LeaveType = newLeaveType;
    }

    public void UpdateStartDate(DateTime newStartDate)
    {
        if (newStartDate <= DateTime.UtcNow)
            throw new LeaveRequestException(LeaveRequestErrorMessages.StartDateShouldNotBeInPast);

        if (newStartDate >= EndDate)
            throw new LeaveRequestException(LeaveRequestErrorMessages.StartDateShouldBeBeforeEndDate);

        StartDate = newStartDate;
    }

    public void UpdateEndDate(DateTime newEndDate)
    {
        if (newEndDate <= StartDate)
            throw new InvalidOperationException(LeaveRequestErrorMessages.EndDateShouldBeAfterStartDate);

        EndDate = newEndDate;
    }

    public void UpdateStatus(
        LeaveRequestStatus newStatus,
        UserId decidedBy,
        string? decisionReason)
    {
        if (decidedBy == null)
            throw new LeaveRequestException(LeaveRequestErrorMessages.ValidUserShouldApproveLeaveRequest);

        Status = newStatus;
        DecidedBy = decidedBy;
        DecisionReason = decisionReason;
    }
}
