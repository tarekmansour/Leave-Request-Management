using System.Diagnostics.CodeAnalysis;
using Domain.Services;
using FluentAssertions;

namespace Domain.Tests;

[ExcludeFromCodeCoverage]
public class EmailValidatorTests
{
    [Theory(DisplayName = "Valid email addresses should return true")]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag+sorting@example.com")]
    [InlineData("user@sub.example.com")]
    [InlineData("user@domain.co")]
    [InlineData("user@domain.com.au")]
    public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);

        // Assert
        result.Should().BeTrue();
    }

    [Theory(DisplayName = "Invalid email addresses should return false")]
    [InlineData("plainaddress")]
    [InlineData("@missingusername.com")]
    [InlineData("username@.com")]
    [InlineData("username@domain,com")]
    [InlineData("username@domain@domain.com")]
    public void IsValidEmail_InvalidEmail_ReturnsFalse(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);

        // Assert
        result.Should().BeFalse();
    }

    [Fact(DisplayName = "Empty email should return false")]
    public void IsValidEmail_EmptyEmail_ReturnsFalse()
    {
        // Act
        var result = EmailValidator.IsValidEmail(string.Empty);

        // Assert
        result.Should().BeFalse();
    }
}
