namespace estore.api.Abstractions.Services;

using estore.api.Common.Pagination;
using estore.api.Common.Results;
using estore.api.Models.Requests;
using estore.api.Models.Responses;

public interface ICustomerServices
{
    Task<Result<CustomerResponse>> GetCustomerByCustomerId(string customerId);

    PagedList<CustomerResponse> GetCustomers(SearchCustomer search);

    Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest customerRequest);
}
