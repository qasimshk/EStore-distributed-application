namespace estore.api.Abstractions.Services;

using estore.api.Common.Results;
using estore.api.Models.Requests;
using estore.api.Models.Responses;

public interface ICustomerServices
{
    Task<Result<CustomerResponse>> GetCustomerByCustomerId(string customerId);

    Task<Result<List<CustomerResponse>>> GetCustomers();

    Task<Result> CreateCustomer(CreateCustomerRequest customerRequest);
}
