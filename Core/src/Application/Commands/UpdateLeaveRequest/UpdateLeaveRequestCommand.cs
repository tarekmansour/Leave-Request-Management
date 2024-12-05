using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequest;

public record UpdateLeaveRequestCommand(
    int LeaveRequestId,
    string Status,
    string? DecisionReason = null) : IRequest<Result<UpdatedLeaveRequestDto>>;
