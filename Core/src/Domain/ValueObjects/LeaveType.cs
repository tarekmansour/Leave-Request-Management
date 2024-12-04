using Domain.Errors;

namespace Domain.ValueObjects;

public record LeaveType
{
    public static readonly LeaveType Off = new("Off");
    public static readonly LeaveType SickLeave = new("SickLeave");
    public static readonly LeaveType Maternity = new("Maternity");
    public static readonly LeaveType Paternity = new("Paternity");
    public static readonly LeaveType MarriageOrPACS = new("MarriageOrPACS");

    public string Value { get; }

    private LeaveType(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    private static readonly Dictionary<string, LeaveType> _leaveTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        { Off.Value, Off },
        { SickLeave.Value, SickLeave },
        { Maternity.Value, Maternity },
        { Paternity.Value, Paternity },
        { MarriageOrPACS.Value, MarriageOrPACS }
    };

    public static LeaveType FromString(string leaveTypeString)
    {
        if (_leaveTypes.TryGetValue(leaveTypeString, out var leaveType))
        {
            return leaveType;
        }
        throw new ArgumentException(LeaveRequestErrorCodes.InvalidLeaveType, nameof(leaveTypeString));
    }

    public static bool IsValidLeaveType(string? leaveTypeString)
    {
        return _leaveTypes.ContainsKey(leaveTypeString!);
    }
}
