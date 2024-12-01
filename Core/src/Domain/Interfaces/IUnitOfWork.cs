namespace Domain.Interfaces;
public interface IUnitOfWork
{
    Task PersistChangesAsync(CancellationToken cancellationToken = default);
}