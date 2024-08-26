namespace estore.api.tests.Fixtures;

using Microsoft.EntityFrameworkCore;

public sealed class MsSqlServerTestContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _connection = null!;
    public EStoreDBContext _dbContext { get; private set; } = null!;

    public MsSqlServerTestContainerFixture() => _msSqlContainer = new MsSqlBuilder()
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
        _dbContext = new EStoreDBContext(dbContextOption.Options);
        _connection = _dbContext.Database.GetDbConnection();

        await _connection.OpenAsync();

        await _dbContext.Database.EnsureCreatedAsync();

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

            _dbContext.Add(categories[row]);
            _dbContext.Add(product);
            _dbContext.Add(supplier[row]);
            _dbContext.Add(customer);
            _dbContext.Add(employee);
            _dbContext.Add(order);
            _dbContext.AddRange(orderDetails);
            row++;
        }
        await _dbContext.SaveChangesAsync();
    }
}
