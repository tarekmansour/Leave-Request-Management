using Application.Abstractions.Authentication;
using Application.Commands.UserLogin;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.Login;
public partial class LoginUserCommandTests : DatabaseFixture
{
    private readonly ILogger<LoginUserCommandHandler> _logger;
    private readonly IValidator<LoginUserCommand> _commandValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly LoginUserCommandHandler _sut;

    public LoginUserCommandTests()
    {
        _logger = Substitute.For<ILogger<LoginUserCommandHandler>>();
        _commandValidator = new LoginUserCommandValidator();
        _userRepository = new UserRepository(_dbContext);
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _tokenProvider = Substitute.For<ITokenProvider>();
        _sut = new LoginUserCommandHandler(_logger, _commandValidator, _userRepository, _passwordHasher, _tokenProvider);
    }
}
