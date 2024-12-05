namespace Api.Contracts;

public record UpdateLeaveRequest(
    LeaveStatus Status,
    string? DecisionReason);

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected
}