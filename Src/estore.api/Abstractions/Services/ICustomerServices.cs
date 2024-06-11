namespace estore.api.Abstractions.Services;

using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;

public interface ICustomerServices
{
    Task<Result<CustomerResponse>> GetCustomerByCustomerId(string customerId);

    PagedList<CustomerResponse> GetCustomers(SearchCustomerRequest search);

    Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest customerRequest);
}
