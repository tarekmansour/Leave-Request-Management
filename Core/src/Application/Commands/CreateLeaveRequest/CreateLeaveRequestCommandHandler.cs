﻿using Application.Dtos;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharedKernel;

namespace Application.Commands.CreateLeaveRequest;
public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Result<CreatedLeaveRequestDto>>
{
    private readonly ILogger _logger;
    private readonly IValidator<CreateLeaveRequestCommand> _commandValidator;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLeaveRequestCommandHandler(
        ILogger<CreateLeaveRequestCommandHandler> logger,
        IValidator<CreateLeaveRequestCommand> commandValidator,
        ILeaveRequestRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = (ILogger)logger ?? NullLogger.Instance;
        _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
        _leaveRequestRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CreatedLeaveRequestDto>> Handle(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));

        var validationResult = await _commandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "New leave request is not valid with the following error codes '{ErrorCodes}'.",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorCode)));

            return Result<CreatedLeaveRequestDto>.Failure(
                validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)));
        }

        var createdLeaveRequestId = await _leaveRequestRepository.CreateLeaveRequestAsync(command.MapToLeaveRequest(), cancellationToken);
        await _unitOfWork.PersistChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created leave request for employee '{EmployeeId}' from start date '{StartDate}' to end date '{EndDate}'.",
                command.EmployeeId,
                command.StartDate,
                command.EndDate);

        return Result<CreatedLeaveRequestDto>.Success(new CreatedLeaveRequestDto(createdLeaveRequestId));
    }
}
