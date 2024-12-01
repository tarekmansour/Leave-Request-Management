using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record LeaveRequestId : StrongTypedId
{
    public LeaveRequestId(int value) : base(value) { }
}
