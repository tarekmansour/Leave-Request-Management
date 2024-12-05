using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRequestRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<LeaveRequestId> CreateAsync(
        LeaveRequest leaveRequest,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.LeaveRequests.AddAsync(leaveRequest, cancellationToken);
        return leaveRequest.Id;
    }

    public async Task<LeaveRequest?> GetByIdAsync(
        LeaveRequestId leaveRequestId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.LeaveRequests.FindAsync(leaveRequestId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<LeaveRequest?>> GetLeaveRequestsAsync(
        UserId userId,
        LeaveRequestStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.LeaveRequests
            .AsNoTracking()
            .Where(x => x.SubmittedBy == userId);

        if (status is not null && !string.IsNullOrEmpty(status.Value))
        {
            query = query.Where(x => x.Status.Value == status.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
