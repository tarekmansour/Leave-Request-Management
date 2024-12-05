using Domain.Entities;
using Domain.ValueObjects.Identifiers;

namespace Domain.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserId> CreateAsync(User user, CancellationToken cancellationToken = default);
}
