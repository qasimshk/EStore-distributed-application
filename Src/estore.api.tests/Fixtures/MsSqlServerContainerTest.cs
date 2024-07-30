namespace estore.api.tests.Fixtures;

using System.Threading.Tasks;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

public sealed class MsSqlServerContainerTest : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;

    public MsSqlServerContainerTest() => _msSqlContainer = new MsSqlBuilder().Build();

    public EStoreDBContext GetDbContext()
    {
        var dbContextBuilder = new DbContextOptionsBuilder<EStoreDBContext>();
        var dbContextOption = dbContextBuilder.UseSqlServer(_msSqlContainer.GetConnectionString());
        return new EStoreDBContext(dbContextOption.Options);
    }

    public Task DisposeAsync() => _msSqlContainer.DisposeAsync().AsTask();

    public Task InitializeAsync() => _msSqlContainer.StartAsync();
}
