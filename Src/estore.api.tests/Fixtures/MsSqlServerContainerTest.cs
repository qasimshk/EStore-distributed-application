namespace estore.api.tests.Fixtures;

using System.Data.Common;
using System.Threading.Tasks;
using estore.api.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

public sealed class MsSqlServerContainerTest : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _connection = null!;
    public EStoreDBContext DbContext { get; private set; } = null!;

    public MsSqlServerContainerTest() => _msSqlContainer = new MsSqlBuilder()
        .WithCleanUp(true)
        .Build();

    public async Task DisposeAsync()
    {
        await _connection.CloseAsync();
        await _msSqlContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var dbContextBuilder = new DbContextOptionsBuilder<EStoreDBContext>();
        var dbContextOption = dbContextBuilder.UseSqlServer(_msSqlContainer.GetConnectionString());
        DbContext = new EStoreDBContext(dbContextOption.Options);
        _connection = DbContext.Database.GetDbConnection();

        await _connection.OpenAsync();

        DbContext.Database.EnsureCreated();

        var customers = CustomerFaker.GetData().Generate(10).ToList();
        var categories = CategoryFaker.GetData().Generate(10).ToList();
        var supplier = SupplierFaker.GetData().Generate(10).ToList();
        var row = 0;

        foreach (var customer in customers)
        {
            var product = ProductFaker.GetData(categories[row].CategoryId, supplier[row].SupplierId).Generate(1).Single();
            var employee = EmployeeFaker.GetData().Generate(1).Single();
            var order = OrderFaker.GetData(customer, employee).Generate(1).Single();
            var orderDetails = OrderDetailsFaker.GetData(order, product.ProductId).Generate(1).ToList();

            DbContext.Add(categories[row]);
            DbContext.Add(product);
            DbContext.Add(supplier[row]);
            DbContext.Add(customer);
            DbContext.Add(employee);
            DbContext.Add(order);
            DbContext.AddRange(orderDetails);
            row++;
        }
        await DbContext.SaveChangesAsync();
    }
}
