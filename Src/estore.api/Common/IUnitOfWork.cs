namespace estore.api.Common;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
