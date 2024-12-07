using System.Diagnostics.CodeAnalysis;
using Application.Commands.UserLogin;
using FluentAssertions;

namespace Application.Tests.Commands.Login;

[ExcludeFromCodeCoverage]
public partial class LoginUserCommandTests
{
    [Fact(DisplayName = "new LoginUserCommandHandler with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new LoginUserCommandHandler(
            _logger,
            _commandValidator,
            null!,
            _passwordHasher,
            _tokenProvider);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new LoginUserCommandHandler with null validator")]
    public void WithNullValidator_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new LoginUserCommandHandler(
            _logger,
            null!,
            _userRepository,
            _passwordHasher,
            _tokenProvider);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
