using Application.Abstractions.Authentication;
using Domain.Errors;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.UserLogin;
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly ILogger _logger;
    private readonly IValidator<LoginUserCommand> _commandValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserCommandHandler(
        ILogger<LoginUserCommandHandler> logger,
        IValidator<LoginUserCommand> commandValidator,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Login user request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<string>.Failure(validationResult.Errors.Select(error => new Error(error.ErrorCode, error.ErrorMessage, ErrorType.Validation)));
        }

        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("The user is not found with email '{Email}'.", command.Email);

            return Result<string>.Failure(new Error(UserErrorCodes.InvalidUserEmail, UserErrorMessages.UserNotFound, ErrorType.NotFound));
        }

        bool verified = _passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified)
        {
            return Result<string>.Failure(new Error(UserErrorCodes.InvalidUserEmail, UserErrorMessages.UserNotFound, ErrorType.Validation));
        }

        string token = _tokenProvider.GenerateToken(user);

        return Result<string>.Success(token);
    }
}
