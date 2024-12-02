using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class TokenProviderTests
{
    private readonly TokenProvider _tokenProvider;

    public TokenProviderTests()
    {
        var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "Jwt:Secret", "my_super_secret_key_which_is_long_enough_32_bytes!" },
            { "Jwt:Issuer", "my_issuer" },
            { "Jwt:Audience", "my_audience" },
            { "Jwt:ExpirationInMinutes", "60" }
        })
        .Build();

        _tokenProvider = new TokenProvider(configuration);
    }

    [Fact(DisplayName = "GenerateToken returns a valid JWT token for a given user")]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var user = new User(new USerId(1), "test@example.com", "FirstName", "LastName");

        // Act
        var token = _tokenProvider.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        handler.CanReadToken(token).Should().BeTrue();

        var jwtToken = handler.ReadJwtToken(token);
        jwtToken.Issuer.Should().Be("my_issuer");
        jwtToken.Audiences.Should().Contain("my_audience");
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
    }
}
