using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record USerId : StrongTypedId
{
    public USerId(int value) : base(value) { }
}
