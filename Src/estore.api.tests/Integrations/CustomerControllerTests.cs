namespace estore.api.tests.Integrations;

using System.Text;

public class CustomerControllerTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactoryFixture _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly EStoreDBContext _dbContext;

    public CustomerControllerTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;

        _httpClient = _factory.Client;

        _dbContext = _factory.Context;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnOkObjectResult_WhenValidCustomerIdSent()
    {
        // Arrange
        var customerId = (await _dbContext.Customers.FirstAsync()).Id.Value;

        // Act        
        var response = await _httpClient.GetAsync($"api/customer/{customerId}");
        var stringResult = await response.Content.ReadAsStringAsync();
        var serviceResponse = JsonSerializer.Deserialize<Result<CustomerResponse>>(stringResult, _jsonSerializerOptions)!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        serviceResponse.Value.Should().NotBeNull();

        serviceResponse.IsSuccess.Should().BeTrue();

        serviceResponse.Value.Should().BeOfType<CustomerResponse>();
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnNotFoundResult_WhenInValidCustomerIdSent()
    {
        // Arrange
        var phoneNumber = (await _dbContext.Customers.FirstAsync()).Phone;

        // Act        
        var response = await _httpClient.GetAsync($"api/customer/{phoneNumber}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomerByPhoneNumber_ShouldReturnOkObjectResult_WhenValidPhoneNumberSent()
    {
        // Arrange
        var phoneNumber = (await _dbContext.Customers.FirstAsync()).Phone;

        // Act
        var response = await _httpClient.GetAsync($"api/customer/phone/{phoneNumber}");
        var stringResult = await response.Content.ReadAsStringAsync();
        var serviceResponse = JsonSerializer.Deserialize<Result<CustomerResponse>>(stringResult, _jsonSerializerOptions)!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        serviceResponse.Value.Should().NotBeNull();

        serviceResponse.IsSuccess.Should().BeTrue();

        serviceResponse.Value.Should().BeOfType<CustomerResponse>();
    }

    [Fact]
    public async Task CustomerSearch_ShouldReturnOkObjectResult_WhenDefaultPaginationParametersSent()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var pagination = /*lang=json,strict*/ "{\"TotalCount\":10,\"PageSize\":10,\"CurrentPage\":1,\"TotalPages\":1,\"HasNext\":false,\"HasPrevious\":false}";

        // Act
        var response = await _httpClient.GetAsync($"/api/customer/search?PageNumber={pageNumber}&PageSize={pageSize}");
        var stringResult = await response.Content.ReadAsStringAsync();
        var serviceResponse = JsonSerializer.Deserialize<PagedList<CustomerResponse>>(stringResult)!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.Headers.GetValues("X-Pagination").First().Should().Be(pagination);

        serviceResponse.Count.Should().Be(pageSize);
    }

    [Fact]
    public async Task CustomerSearch_ShouldReturnOkObjectResult_WhenPaginationAndSearchParametersSent()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var pagination = /*lang=json,strict*/ "{\"TotalCount\":10,\"PageSize\":10,\"CurrentPage\":1,\"TotalPages\":1,\"HasNext\":false,\"HasPrevious\":false}";

        // Act
        var response = await _httpClient.GetAsync($"/api/customer/search?PageNumber={pageNumber}&PageSize={pageSize}&ContactTitle=Software%20engineer");
        var stringResult = await response.Content.ReadAsStringAsync();
        var serviceResponse = JsonSerializer.Deserialize<PagedList<CustomerResponse>>(stringResult)!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.Headers.GetValues("X-Pagination").First().Should().Be(pagination);

        serviceResponse.Count.Should().Be(pageSize);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnTrue_WhenCreateCustomerRequestSent()
    {
        // Arrange
        var customer = CustomerFaker.GetData().Generate(1).Single();

        var serviceRequest = GetRequestToStringConstant(new CreateCustomerRequest
        {
            Address = customer.CustomerAddress.Address,
            City = customer.CustomerAddress.City,
            Region = customer.CustomerAddress.Region,
            CompanyName = customer.CompanyName,
            ContactName = customer.ContactName,
            ContactTitle = customer.ContactTitle,
            Country = customer.CustomerAddress.Country,
            Fax = customer.Fax,
            Phone = customer.Phone,
            PostalCode = customer.CustomerAddress.PostalCode
        });

        // Act
        var response = await _httpClient.PostAsync($"/api/customer", serviceRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private static StringContent GetRequestToStringConstant(object request)
    {
        var customer = JsonSerializer.Serialize(request);

        return new StringContent(customer, Encoding.UTF8, "application/json");
    }
}
