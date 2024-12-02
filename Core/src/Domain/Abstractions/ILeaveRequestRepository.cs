using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Domain.Interfaces;
public interface ILeaveRequestRepository
{
    Task<LeaveRequestId> CreateLeaveRequestAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default);
    Task<LeaveRequest?> GetLeaveRequestByIdAsync(LeaveRequestId leaveRequestId, CancellationToken cancellationToken = default);
}
