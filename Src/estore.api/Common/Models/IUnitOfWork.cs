namespace estore.api.Common.Models;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
