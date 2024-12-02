using Application.Dtos;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using MediatR;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequestStatus;

public record UpdateLeaveRequestStatusCommand(
    LeaveRequestId LeaveRequestId,
    LeaveRequestStatus NewStatus,
    string? DecisionReason = null) : IRequest<Result<UpdatedLeaveRequestDto>>;
