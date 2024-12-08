using System.Diagnostics.CodeAnalysis;
using Application.Abstractions.Authentication;
using Application.Commands.UserRegister;
using Domain.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedKernel.Tests;

namespace Application.Tests.Commands.Register;

[ExcludeFromCodeCoverage]
public partial class RegisterUserCommandTests : DatabaseFixture
{
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IValidator<RegisterUserCommand> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegisterUserCommandHandler _sut;

    public RegisterUserCommandTests()
    {
        _logger = Substitute.For<ILogger<RegisterUserCommandHandler>>();
        _validator = new RegisterUserCommandValidator();
        _userRepository = new UserRepository(_dbContext);
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = new UnitOfWork(_dbContext);
        _sut = new RegisterUserCommandHandler(_logger, _validator, _userRepository, _passwordHasher, _unitOfWork);
    }
}
