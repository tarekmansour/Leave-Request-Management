using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Infrastructure.Authentication;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact(DisplayName = "Hash should generate a valid hash for a given password")]
    public void Hash_ShouldGenerateValidHash_WhenGivenPassword()
    {
        // Arrange
        string password = "SecurePassword123!";

        // Act
        string passwordHash = _passwordHasher.Hash(password);

        // Assert
        passwordHash.Should().NotBeNullOrEmpty();
        passwordHash.Should().Contain("-");
        passwordHash.Split('-').Should().HaveCount(2);
    }

    [Fact(DisplayName = "Verify should return true for a valid password and hash")]
    public void Verify_ShouldReturnTrue_WhenPasswordIsValid()
    {
        // Arrange
        string password = "SecurePassword123!";
        string passwordHash = _passwordHasher.Hash(password);

        // Act
        bool result = _passwordHasher.Verify(password, passwordHash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Verify should return false for an invalid password")]
    public void Verify_ShouldReturnFalse_WhenPasswordIsInvalid()
    {
        // Arrange
        string password = "SecurePassword123!";
        string wrongPassword = "WrongPassword!";
        string passwordHash = _passwordHasher.Hash(password);

        // Act
        bool result = _passwordHasher.Verify(wrongPassword, passwordHash);

        // Assert
        result.Should().BeFalse();
    }
}
