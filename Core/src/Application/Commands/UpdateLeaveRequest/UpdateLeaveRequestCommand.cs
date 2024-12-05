using Application.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Commands.UpdateLeaveRequest;

public record UpdateLeaveRequestCommand(
    int LeaveRequestId,
    string? LeaveType = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? Status = null,
    string? Comment = null,
    string? DecisionReason = null) : IRequest<Result<UpdatedLeaveRequestDto>>;
