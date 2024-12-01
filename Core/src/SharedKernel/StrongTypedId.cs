namespace SharedKernel;

public abstract record StrongTypedId
{
    public int Value { get; }

    protected StrongTypedId(int value)
    {
        if (value <= 0)
            throw new ArgumentException(SharedErrorMessages.IdMustBeGraterThanZero, nameof(value));
        Value = value;
    }
}
