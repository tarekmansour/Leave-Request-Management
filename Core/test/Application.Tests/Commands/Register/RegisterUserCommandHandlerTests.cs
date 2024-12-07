using Application.Commands.UserRegister;
using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using FluentAssertions;
using Infrastructure.Authentication;

namespace Application.Tests.Commands.Register;
public partial class RegisterUserCommandTests
{
    [Fact(DisplayName = "new RegisterUserCommand with null repository")]
    public void WithNullRepository_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new RegisterUserCommandHandler(
            _logger,
            _validator,
            null!,
            _passwordHasher,
            _unitOfWork);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "new RegisterUserCommand with null unitOfWork")]
    public void WithNullUnitOfWork_Should_ThrowException()
    {
        // Arrange & act
        var act = () => new RegisterUserCommandHandler(
            _logger,
            _validator,
            _userRepository!,
            _passwordHasher,
            null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Handle returns successful result")]
    public async Task Handle_Should_ReturnsSuccessfulResult()
    {
        //Arrange
        var command = new RegisterUserCommand(
            Email: "test@gmail.com",
            FirstName: "toto",
            LastName: "tata",
            Password: "12345678",
            Roles: [Roles.Employee, Roles.HR]);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact(DisplayName = "Handle returns failure result with existing user")]
    public async Task Handle_Should_ReturnsFailureResult()
    {
        //Arrange
        await _dbContext.Users.AddAsync(new User(
            id: new UserId(2),
            email: "test@gmail.com",
            firstName: "test1",
            lastName: "toto",
            passwordHash: "123",
            roles: [Roles.Employee, Roles.HR]));
        await _dbContext.SaveChangesAsync();

        var command = new RegisterUserCommand(
            Email: "test@gmail.com",
            FirstName: "test2",
            LastName: "tata",
            Password: "12345678",
            Roles: [Roles.Employee, Roles.HR]);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
    }
}
