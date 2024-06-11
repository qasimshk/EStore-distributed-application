namespace estore.api.Common;

using System.Threading;
using System.Threading.Tasks;
using estore.api.Persistance.Context;

public class UnitOfWork(EStoreDBContext eStoreDBContext) : IUnitOfWork
{
    private readonly EStoreDBContext _eStoreDBContext = eStoreDBContext;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) =>
        await _eStoreDBContext.SaveChangesAsync(cancellationToken);

    public void Dispose() => _eStoreDBContext.Dispose();
}
