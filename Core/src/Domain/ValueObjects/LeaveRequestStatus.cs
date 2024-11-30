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
}
