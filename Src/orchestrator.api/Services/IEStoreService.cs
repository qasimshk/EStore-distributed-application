namespace orchestrator.api.Services;

using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;

public interface IEStoreService
{
    Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest request);

    Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request);

    Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId);

    Task<Result> DeleteCustomer(string customerId);

    Task<Result> DeleteOrder(int orderId);
}
