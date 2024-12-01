namespace Domain.ValueObjects;

public record EmployeePosition
{
    public static readonly EmployeePosition Manager = new("Manager");
    public static readonly EmployeePosition HR = new("HR");
    public static readonly EmployeePosition Employee = new("Employee");
    public string Value { get; }

    private EmployeePosition(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}