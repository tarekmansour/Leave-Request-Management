namespace Api.Contracts;

public record UpdateLeaveRequest(
    LeaveType? LeaveType,
    DateTime? StartDate,
    DateTime? EndDate,
    LeaveStatus? NewStatus,
    string? Comment,
    string? DecisionReason);

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected
}