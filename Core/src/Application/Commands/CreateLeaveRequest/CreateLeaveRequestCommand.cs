using MediatR;
using SharedKernel;

namespace Application.Commands.CreateLeaveRequest;
public record CreateLeaveRequestCommand(
    string LeaveType,
    DateTime StartDate,
    DateTime EndDate,
    string? Comment = null) : IRequest<Result<int>>;
