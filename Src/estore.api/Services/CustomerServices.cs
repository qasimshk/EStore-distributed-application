namespace estore.api.Services;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Common.Results;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Requests;
using estore.api.Models.Responses;
using Models.Aggregates.Customer.ValueObjects;

public class CustomerServices(
    ICustomerRepository customerRepository,
    ICustomerMapper customermaMapper) : ICustomerServices
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerMapper _customermaMapper = customermaMapper;

    public Task<Result> CreateCustomer(CreateCustomerRequest customerRequest) => throw new NotImplementedException();

    public async Task<Result<CustomerResponse>> GetCustomerByCustomerId(string customerId)
    {
        var customer = await _customerRepository
            .FindByConditionAsync(x => x.Id == new CustomerId(customerId));

        return customer.Any() ?
           Result<CustomerResponse>
            .SuccessResult(_customermaMapper.Map(customer.SingleOrDefault()!)) :
           Result<CustomerResponse>
            .FailedResult("Customer not found with this Id", HttpStatusCode.NotFound);
    }

    public Task<Result<List<CustomerResponse>>> GetCustomers() => throw new NotImplementedException();
}
