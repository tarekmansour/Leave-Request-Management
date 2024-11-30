using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record EmployeeId : StrongTypedId
{
    public EmployeeId(int value) : base(value) { }
}
