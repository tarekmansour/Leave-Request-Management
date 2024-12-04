using Application.Dtos;
using Domain.Entities;
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

    public UpdateLeaveRequestCommandHandler(
        ILogger<UpdateLeaveRequestCommandHandler> logger,
        IValidator<UpdateLeaveRequestCommand> commandValidator,
        ILeaveRequestRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _leaveRequestRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<UpdatedLeaveRequestDto>> Handle(UpdateLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Updating leave request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<UpdatedLeaveRequestDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var existingLeaveRequest = await _leaveRequestRepository.GetByIdAsync(new LeaveRequestId(command.LeaveRequestId), cancellationToken);

        if (existingLeaveRequest is null)
        {
            _logger.LogWarning("The leave request to update is not found for LeaveRequestId '{LeaveRequestId}'.", command.LeaveRequestId);
            return Result<UpdatedLeaveRequestDto>.Failure(new Error(LeaveRequestErrorCodes.InvalidLeaveRequestId, LeaveRequestErrorMessages.NotFoundLeaveRequestToUpdate));
        }

        ApplyUpdates(command, existingLeaveRequest);

        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated leave request '{LeaveRequestId}' with new status '{NewStatus}'.",
                command.LeaveRequestId,
                command.NewStatus);

        return Result<UpdatedLeaveRequestDto>.Success(new UpdatedLeaveRequestDto(
            Id: existingLeaveRequest.Id.Value,
            SubmittedBy: existingLeaveRequest.SubmittedBy.Value,
            LeaveType: existingLeaveRequest.LeaveType.ToString(),
            StartDate: existingLeaveRequest.StartDate,
            EndDate: existingLeaveRequest.EndDate,
            Status: existingLeaveRequest.Status.ToString(),
            DecisionReason: existingLeaveRequest.DecisionReason));
    }

    public static void ApplyUpdates(UpdateLeaveRequestCommand command, LeaveRequest leaveRequest)
    {
        if (!string.IsNullOrWhiteSpace(command.LeaveType))
            leaveRequest.UpdateLeaveType(LeaveType.FromString(command.LeaveType));

        if (command.StartDate.HasValue)
            leaveRequest.UpdateStartDate(command.StartDate.Value);

        if (command.EndDate.HasValue)
            leaveRequest.UpdateEndDate(command.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(command.NewStatus))
            leaveRequest.UpdateStatus(
                LeaveRequestStatus.FromString(command.NewStatus),
                new UserId(1),
                command.DecisionReason);
    }
}
