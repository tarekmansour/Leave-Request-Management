using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class LeaveType
{
    public LeaveTypeId Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public int MaxDaysPerYear { get; private set; } = default!;

    public LeaveType() { } // For EF Core

    public LeaveType(string name, int maxDaysPerYear)
    {
        Name = name;
        MaxDaysPerYear = maxDaysPerYear;
    }

    public LeaveType(LeaveTypeId id, string name, int maxDaysPerYear)
    {
        Id = id;
        Name = name;
        MaxDaysPerYear = maxDaysPerYear;
    }
}
