using SharedKernel;

namespace Domain.ValueObjects.Identifiers;

public record UserId : StrongTypedId
{
    public UserId(int value) : base(value) { }
}
