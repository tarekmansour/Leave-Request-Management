using Application.Abstractions;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Result<int>>
{
    private readonly ILogger _logger;
    private readonly IValidator<CreateLeaveRequestCommand> _commandValidator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateLeaveRequestCommandHandler(
        ILogger<CreateLeaveRequestCommandHandler> logger,
        IValidator<CreateLeaveRequestCommand> commandValidator,
        ILeaveRequestRepository leaveRequestRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    public async Task<Result<int>> Handle(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New leave request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<int>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var createdLeaveRequestId = await _leaveRequestRepository.CreateAsync(command.MapToLeaveRequest(_userContext.UserId), cancellationToken);
        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created leave request for user '{UserId}' from start date '{StartDate}' to end date '{EndDate}'.",
                _userContext.UserId,
                command.StartDate,
                command.EndDate);

        return Result<int>.Success(createdLeaveRequestId.Value);
    }
}
