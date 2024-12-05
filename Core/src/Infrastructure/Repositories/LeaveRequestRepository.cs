using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRequestRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<LeaveRequestId> CreateAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default)
    {
        await _dbContext.LeaveRequests.AddAsync(leaveRequest, cancellationToken);
        return leaveRequest.Id;
    }

    public async Task<LeaveRequest?> GetByIdAsync(LeaveRequestId leaveRequestId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LeaveRequests.FindAsync(leaveRequestId, cancellationToken);
    }

    public async Task<IEnumerable<LeaveRequest?>> GetLeaveRequestsAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LeaveRequests.Where(x => x.SubmittedBy == userId).ToListAsync(cancellationToken);
    }
}
