using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identifiers;

namespace Infrastructure.Repositories;
public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRequestRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<LeaveRequestId> CreateLeaveRequestAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default)
    {
        await _dbContext.LeaveRequests.AddAsync(leaveRequest, cancellationToken);
        return leaveRequest.Id;
    }
}
