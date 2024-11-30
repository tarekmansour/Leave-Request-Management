using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record LeaveTypeId : StrongTypedId
{
    public LeaveTypeId(int value) : base(value) { }
}