using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Errors;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequest;
public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Result<UpdatedLeaveRequestDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<UpdateLeaveRequestCommand> _commandValidator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMessageSender _messageSender;

    public UpdateLeaveRequestCommandHandler(
        ILogger<UpdateLeaveRequestCommandHandler> logger,
        IValidator<UpdateLeaveRequestCommand> commandValidator,
        ILeaveRequestRepository appointmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMessageSender messageSender)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _leaveRequestRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
    }

    public async Task<Result<UpdatedLeaveRequestDto>> Handle(UpdateLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Updating leave request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<UpdatedLeaveRequestDto>.Failure(validationResult.Errors.Select(error => new Error(error.ErrorCode, error.ErrorMessage, ErrorType.Validation)));
        }

        var existingLeaveRequest = await _leaveRequestRepository.GetByIdAsync(new LeaveRequestId(command.LeaveRequestId), cancellationToken);

        if (existingLeaveRequest is null)
        {
            _logger.LogWarning("The leave request to update is not found for LeaveRequestId '{LeaveRequestId}'.", command.LeaveRequestId);

            return Result<UpdatedLeaveRequestDto>.Failure(new Error(
                LeaveRequestErrorCodes.InvalidLeaveRequestId,
                LeaveRequestErrorMessages.NotFoundLeaveRequestToUpdate,
                ErrorType.NotFound));
        }

        existingLeaveRequest.UpdateStatus(
            LeaveRequestStatus.FromString(command.Status),
            new UserId(_userContext.UserId),
            command.DecisionReason);

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated leave request '{LeaveRequestId}' with new status '{NewStatus}'.",
                command.LeaveRequestId,
                command.Status);

        _messageSender.SendMessage(new LeaveRequestStatusChangedDto(
            existingLeaveRequest.Id.Value,
            existingLeaveRequest.Status.Value));

        return Result<UpdatedLeaveRequestDto>.Success(existingLeaveRequest.MapToUpdatedLeaveRequestDto());
    }
}
