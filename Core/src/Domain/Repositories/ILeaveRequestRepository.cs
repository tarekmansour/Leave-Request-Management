using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Domain.Repositories;
public interface ILeaveRequestRepository
{
    Task<LeaveRequestId> CreateAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default);
    Task<LeaveRequest?> GetByIdAsync(LeaveRequestId leaveRequestId, CancellationToken cancellationToken = default);
}
