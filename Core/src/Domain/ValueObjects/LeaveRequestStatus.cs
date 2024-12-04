using Domain.Errors;

namespace Domain.ValueObjects;

public record LeaveRequestStatus
{
    public static readonly LeaveRequestStatus Pending = new("Pending");
    public static readonly LeaveRequestStatus Approved = new("Approved");
    public static readonly LeaveRequestStatus Rejected = new("Rejected");
    public string Value { get; }

    private LeaveRequestStatus(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    private static readonly Dictionary<string, LeaveRequestStatus> _leaveRequestStatus = new(StringComparer.OrdinalIgnoreCase)
    {
        { Pending.Value, Pending },
        { Approved.Value, Approved },
        { Rejected.Value, Rejected }
    };

    public static LeaveRequestStatus FromString(string leaveRequestStatusString)
    {
        if (_leaveRequestStatus.TryGetValue(leaveRequestStatusString, out var leaveRequestStatus))
        {
            return leaveRequestStatus;
        }
        throw new ArgumentException(LeaveRequestErrorCodes.InvalidLeaveRequestStatus, nameof(leaveRequestStatusString));
    }

    public static bool IsValidLeaveRequestStatus(string? leaveRequestStatusString)
    {
        return _leaveRequestStatus.ContainsKey(leaveRequestStatusString!);
    }
}
