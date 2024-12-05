using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AnyAsync(u => u.Id.Value == id, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase), cancellationToken);

    public async Task<UserId> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(user, cancellationToken);
        return user.Id;
    }
}
