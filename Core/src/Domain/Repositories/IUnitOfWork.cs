namespace Domain.Repositories;
public interface IUnitOfWork
{
    Task PersistChangesAsync(CancellationToken cancellationToken = default);
}