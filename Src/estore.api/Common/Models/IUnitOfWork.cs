namespace estore.api.Common.Models;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
