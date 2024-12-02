using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Repositories;
using SharedKernel.Tests;

namespace Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests : DatabaseFixture
{
    private readonly IUserRepository _sut;

    public UserRepositoryTests() => _sut = new UserRepository(_dbContext);

    [Fact(DisplayName = "Return user by email")]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User(new UserId(1), "test@example.com", "John", "Doe", "hashedPassword");
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByEmailAsync("test@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }

    [Fact(DisplayName = "Return null when user does not exists")]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Act
        var result = await _sut.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Add user to db")]
    public async Task CreateAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = new User("newuser@example.com", "Jane", "Doe", "hashedPassword");

        // Act
        var userId = await _sut.CreateAsync(user);
        await _dbContext.SaveChangesAsync();

        // Assert
        userId.Should().NotBeNull();
        var retrievedUser = await _sut.GetByEmailAsync("newuser@example.com");
        retrievedUser.Should().NotBeNull();
        retrievedUser!.Email.Should().Be("newuser@example.com");
    }
}
