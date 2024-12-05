using Application.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.UserRegister;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<int>>
{
    private readonly ILogger _logger;
    private readonly IValidator<RegisterUserCommand> _commandValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IValidator<RegisterUserCommand> commandValidator,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Result<int>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Register login request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<int>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var userExists = await _userRepository.ExistsByEmailAsync(command.Email, cancellationToken);

        if (userExists)
        {
            _logger.LogWarning("The user email is not unique with email '{Email}'.", command.Email);
            return Result<int>.Failure(new Error(UserErrorCodes.InvalidUserEmail, UserErrorMessages.UserEmailNotUnique));
        }

        var createdUserId = await _userRepository.CreateAsync(
            new User(
                email: command.Email,
                firstName: command.FirstName,
                lastName: command.LastName,
                passwordHash: _passwordHasher.Hash(command.Password),
                roles: command.Roles),
            cancellationToken);

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully registered new user with first name '{FirstName}', last name '{LastName}' and email '{Email}'.",
                command.FirstName,
                command.LastName,
                command.Email);

        return Result<int>.Success(createdUserId.Value);
    }
}
