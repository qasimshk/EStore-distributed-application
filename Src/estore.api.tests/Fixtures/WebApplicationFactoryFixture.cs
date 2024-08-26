namespace estore.api.tests.Fixtures;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

public class WebApplicationFactoryFixture : IAsyncLifetime
{
    public HttpClient Client { get; private set; }
    public EStoreDBContext Context { get; private set; }
    private WebApplicationFactory<Program> _factory;
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _connection = null!;

    public WebApplicationFactoryFixture() => _msSqlContainer = new MsSqlBuilder()
            .WithCleanUp(true)
            .Build();

    public async Task InitializeAsync()
    {
        //Start Container
        await _msSqlContainer.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<EStoreDBContext>>();

                    services.AddDbContext<EStoreDBContext>(options =>
                       options.UseSqlServer(_msSqlContainer.GetConnectionString()));
                });
                builder.UseEnvironment("Test");
            });

        // Initialize database
        var scope = _factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<EStoreDBContext>();
        _connection = Context.Database.GetDbConnection();

        await _connection.OpenAsync();

        await Context.Database.EnsureCreatedAsync();

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

            Context.Add(categories[row]);
            Context.Add(product);
            Context.Add(supplier[row]);
            Context.Add(customer);
            Context.Add(employee);
            Context.Add(order);
            Context.AddRange(orderDetails);
            row++;
        }
        await Context.SaveChangesAsync();

        Client = _factory.CreateDefaultClient();
    }

    public async Task DisposeAsync()
    {
        await _connection.CloseAsync();
        await _msSqlContainer.DisposeAsync();
    }
}
