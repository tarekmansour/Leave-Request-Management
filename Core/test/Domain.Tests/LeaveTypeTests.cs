using System.Diagnostics.CodeAnalysis;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests;

[ExcludeFromCodeCoverage]
public class LeaveTypeTests
{
    [Theory(DisplayName = "FromString returns valid type")]
    [InlineData("Off", "Off")]
    [InlineData("SickLeave", "SickLeave")]
    [InlineData("Maternity", "Maternity")]
    [InlineData("Paternity", "Paternity")]
    [InlineData("MarriageOrPACS", "MarriageOrPACS")]
    public void FromString_ShouldReturnLeaveType_WhenInputIsValid(string input, string expectedValue)
    {
        // Act
        var result = LeaveType.FromString(input);

        // Assert
        result.Value.Should().Be(expectedValue);
    }

    [Fact(DisplayName = "FromString throw ArgumentException when input is invalid")]
    public void FromString_ShouldThrowArgumentException_WhenInputIsInvalid()
    {
        // Arrange
        var invalidInput = "Holiday";

        // Act
        Action act = () => LeaveType.FromString(invalidInput);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("*Invalid_LeaveType*")
           .And.ParamName.Should().Be("leaveTypeString");
    }

    [Theory(DisplayName = "IsValidLeaveType returns True")]
    [InlineData("Off")]
    [InlineData("SickLeave")]
    [InlineData("Maternity")]
    [InlineData("Paternity")]
    [InlineData("MarriageOrPACS")]
    public void IsValidLeaveType_ShouldReturnTrue_WhenInputIsValid(string input)
    {
        // Act
        var result = LeaveType.IsValidLeaveType(input);

        // Assert
        result.Should().BeTrue();
    }

    [Theory(DisplayName = "IsValidLeaveType returns False")]
    [InlineData("Holiday")]
    [InlineData("Vacation")]
    [InlineData("")]
    public void IsValidLeaveType_ShouldReturnFalse_WhenInputIsInvalid(string input)
    {
        // Act
        var result = LeaveType.IsValidLeaveType(input);

        // Assert
        result.Should().BeFalse();
    }
}
