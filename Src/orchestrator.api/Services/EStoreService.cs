namespace orchestrator.api.Services;

using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using System.Text;
using System.Text.Json;

public class EStoreService(HttpClient httpClient) : IEStoreService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = JsonSerializer.Serialize(request);

        var requestContent = new StringContent(customer, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Customer", requestContent);

        return (await response.Content.ReadFromJsonAsync<Result<CreateCustomerResponse>>())!;
    }

    public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request)
    {
        var order = JsonSerializer.Serialize(request);

        var requestContent = new StringContent(order, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("Order", requestContent);

        var content = await response.Content.ReadAsStringAsync();

        return (await response.Content.ReadFromJsonAsync<Result<OrderResponse>>())!;
    }

    public async Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId)
    {
        var response = await _httpClient.GetAsync($"Employee/{employeeId}");

        return (await response.Content.ReadFromJsonAsync<Result<EmployeeResponse>>())!;
    }
}
