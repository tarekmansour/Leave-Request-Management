using Domain.ValueObjects.Identifiers;

namespace Domain.Entities;
public sealed class LeaveType
{
    public LeaveTypeId Id { get; private set; } = default!;
    public string Name { get; private set; }
    public int MaxDaysPerYear { get; private set; }

    public LeaveType(string name, int maxDaysPerYear)
    {
        Name = name;
        MaxDaysPerYear = maxDaysPerYear;
    }
}
