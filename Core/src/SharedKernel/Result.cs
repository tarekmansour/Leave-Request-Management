namespace SharedKernel;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; } = default!;
    public IReadOnlyList<Error> Errors { get; } = new List<Error>();

    public bool IsFailure => !IsSuccess;

    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException(SharedErrorMessages.SuccessfulResultCannotHaveError);

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException(SharedErrorMessages.FailedResultMustHaveError);

        IsSuccess = isSuccess;
        Error = error;
    }

    public Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        if (isSuccess && errors.Count > 0)
            throw new InvalidOperationException(SharedErrorMessages.SuccessfulResultCannotHaveErrors);

        if (!isSuccess && errors.Count == 0)
            throw new InvalidOperationException(SharedErrorMessages.FailedResultMustHaveErrors);

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result Failure(IEnumerable<Error> errors) => new Result(false, errors.ToList());
}

public class Result<T> : Result
{
    public T Value { get; }

    public Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        if (isSuccess && value is null)
        {
            throw new InvalidOperationException(SharedErrorMessages.SuccessfulResultMustHaveValue);
        }

        Value = value;
    }

    public Result(T value, bool isSuccess, IReadOnlyList<Error> errors)
        : base(isSuccess, errors)
    {
        if (isSuccess && value is null)
            throw new InvalidOperationException(SharedErrorMessages.SuccessfulResultMustHaveValue);

        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, Error.None);
    public new static Result<T> Failure(Error error) => new Result<T>(default!, false, error);
    public new static Result<T> Failure(IEnumerable<Error> errors) => new Result<T>(default!, false, errors.ToList());
}
