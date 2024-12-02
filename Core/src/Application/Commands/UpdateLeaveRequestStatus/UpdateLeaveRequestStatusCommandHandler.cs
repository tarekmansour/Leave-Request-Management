using Application.Dtos;
using Domain.Errors;
using Domain.Repositories;
using Domain.ValueObjects.Identifiers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequestStatus;
public class UpdateLeaveRequestStatusCommandHandler : IRequestHandler<UpdateLeaveRequestStatusCommand, Result<UpdatedLeaveRequestDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<UpdateLeaveRequestStatusCommand> _commandValidator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLeaveRequestStatusCommandHandler(
        ILogger<UpdateLeaveRequestStatusCommandHandler> logger,
        IValidator<UpdateLeaveRequestStatusCommand> commandValidator,
        ILeaveRequestRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _leaveRequestRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<UpdatedLeaveRequestDto>> Handle(UpdateLeaveRequestStatusCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New leave request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<UpdatedLeaveRequestDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var existingLeaveRequest = await _leaveRequestRepository.GetByIdAsync(command.LeaveRequestId, cancellationToken);

        if (existingLeaveRequest is null)
        {
            _logger.LogWarning("The leave request to update is not found for LeaveRequestId '{LeaveRequestId}'.", command.LeaveRequestId);
            return Result<UpdatedLeaveRequestDto>.Failure(new Error(LeaveRequestErrorCodes.InvalidLeaveRequestId, LeaveRequestErrorMessages.NotFoundLeaveRequestToUpdate));
        }

        existingLeaveRequest.UpdateStatus(
            status: command.NewStatus,
            decidedBy: new UserId(1),//todo: get id of validator
            decisionReason: command.DecisionReason);

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated leave request '{LeaveRequestId}' with new status '{NewStatus}'.",
                command.LeaveRequestId,
                command.NewStatus);

        return Result<UpdatedLeaveRequestDto>.Success(new UpdatedLeaveRequestDto(
            Id: existingLeaveRequest.Id,
            SubmittedBy: existingLeaveRequest.SubmittedBy,
            StartDate: existingLeaveRequest.StartDate,
            EndDate: existingLeaveRequest.EndDate,
            Status: existingLeaveRequest.Status.ToString(),
            DecisionReason: existingLeaveRequest.DecisionReason));
    }
}
