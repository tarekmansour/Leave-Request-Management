using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record UpdateLeaveRequest(
    [Required] int LeaveRequestId,
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