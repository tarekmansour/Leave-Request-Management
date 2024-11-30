using FluentAssertions;

namespace SharedKernel.Tests;
public class StrongTypedIdTests
{
    [Fact(DisplayName = "Should create StrongTypedId with valid positive value")]
    public void New_ShouldCreateStrongTypedId_WhenValueIsGreaterThanZero()
    {
        // Arrange
        int validValue = 1;

        // Act
        var strongTypedId = new TestStrongTypedId(validValue);

        // Assert
        strongTypedId.Should().NotBeNull();
        strongTypedId.Should().NotBeSameAs(validValue);
    }

    [Fact(DisplayName = "Should throw ArgumentException when value is zero")]
    public void New_ShouldThrowArgumentException_WhenValueIsZero()
    {
        // Arrange
        int invalidValue = 0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TestStrongTypedId(invalidValue));
    }

    private record TestStrongTypedId : StrongTypedId
    {
        public TestStrongTypedId(int value) : base(value) { }
    }
}
