using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace SharedKernel.Tests;

[ExcludeFromCodeCoverage]
public class ResultTests
{
    [Fact(DisplayName = "Success Result should indicate success and have no error")]
    public void Success_Should_IndicateSuccess_And_HaveNoError()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact(DisplayName = "Failure Result should indicate failure and contain the specified error")]
    public void Failure_Should_IndicateFailure_And_ContainSpecifiedError()
    {
        // Arrange
        var error = new Error("error_code", "An error occurred", ErrorType.Validation);

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact(DisplayName = "Creating a Success Result with an error should throw an exception")]
    public void Success_WithError_ShouldThrowException()
    {
        // Arrange
        var error = new Error("error_code", "An error occurred", ErrorType.Failure);

        // Act
        var act = () => new Result(true, error);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A successful result cannot have an error.");
    }

    [Fact(DisplayName = "Creating a Failure Result without an error should throw an exception")]
    public void Failure_WithoutError_ShouldThrowException()
    {
        // Act
        var act = () => new Result(false, Error.None);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A failed result must have an error.");
    }

    [Fact(DisplayName = "Generic Success Result should indicate success and contain the value")]
    public void GenericSuccess_Should_IndicateSuccess_And_ContainValue()
    {
        // Arrange
        var value = "Test Value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Error.Should().Be(Error.None);
    }

    [Fact(DisplayName = "Generic Failure Result should indicate failure and contain the specified error")]
    public void GenericFailure_Should_IndicateFailure_And_ContainSpecifiedError()
    {
        // Arrange
        var error = new Error("error_code", "A generic error occurred", ErrorType.Failure);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact(DisplayName = "Creating a Generic Success Result with null value should throw an exception")]
    public void GenericSuccess_WithNullValue_ShouldThrowException()
    {
        // Act
        var act = () => Result<string>.Success(null!);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A successful result must have a value.");
    }
}
