using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class UserContextTests
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserContext _userContext;

    public UserContextTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _userContext = new UserContext(_httpContextAccessor);
    }

    [Fact(DisplayName = "User Id should return the correct user Id when available")]
    public void UserId_ShouldReturnCorrectUserId_WhenAvailable()
    {
        // Arrange
        var userId = 123;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _userContext.UserId;

        // Assert
        result.Should().Be(userId);
    }

    [Fact(DisplayName = "User Id should throw an exception when user context is unavailable")]
    public void UserId_ShouldThrowException_WhenUserContextIsUnavailable()
    {
        // Arrange
        _httpContextAccessor.HttpContext.Returns((HttpContext)null);

        // Act
        Func<int> act = () => _userContext.UserId;

        // Assert
        act.Should().Throw<ApplicationException>()
            .WithMessage("User context is unavailable");
    }

    [Fact(DisplayName = "User Id should throw an exception when user Id is not available")]
    public void UserId_ShouldThrowException_WhenUserIdIsNotAvailable()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "not-an-integer")
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        Func<int> act = () => _userContext.UserId;

        // Assert
        act.Should().Throw<ApplicationException>()
            .WithMessage("User id is unavailable");
    }
}
