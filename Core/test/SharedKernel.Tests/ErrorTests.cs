using FluentAssertions;

namespace SharedKernel.Tests;
public class ErrorTests
{
    [Fact(DisplayName = "New error object")]
    public void NewError_Should_CreateError()
    {
        // Arrange
        var code = "code";
        var description = "description";

        // Act
        var result = new Error(code, description);

        // Assert
        result.Code.Should().Be(code);
        result.Description.Should().Be(description);
    }

    [Fact(DisplayName = "Error.None should have an empty code and description")]
    public void Error_None_ShouldHaveEmptyCodeAndDescription()
    {
        // Act
        var error = Error.None;

        // Assert
        error.Code.Should().Be(string.Empty);
        error.Description.Should().Be(string.Empty);
    }
}
