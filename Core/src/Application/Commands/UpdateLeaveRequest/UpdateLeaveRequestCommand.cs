using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequest;

public record UpdateLeaveRequestCommand(
    int LeaveRequestId,
    string? LeaveType,
    DateTime? StartDate,
    DateTime? EndDate,
    string? NewStatus,
    string? Comment,
    string? DecisionReason = null) : IRequest<Result<UpdatedLeaveRequestDto>>;
