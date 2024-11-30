namespace SharedKernel;
public static class SharedErrorMessages
{
    public static readonly string SuccessfulResultCannotHaveError = "A successful result cannot have an error.";
    public static readonly string SuccessfulResultCannotHaveErrors = "A successful result cannot have an errors.";
    public static readonly string SuccessfulResultMustHaveValue = "A successful result must have a value.";

    public static readonly string FailedResultMustHaveError = "A failed result must have an error.";
    public static readonly string FailedResultMustHaveErrors = "A failed result must have at least one error.";

    public static readonly string IdMustBeGraterThanZero = "Id must be greater than zero.";
}
