namespace estore.api.Abstractions.Mappers;

using estore.api.Common;
using estore.api.Models.Aggregates.Customer;
using estore.common.Models.Responses;
using estore.common.Models.Requests;

public interface ICustomerMapper :
    IMapper<Customer, CustomerResponse>,
    IMapper<CreateCustomerRequest, Customer>
{
    CreateCustomerResponse Mapper(Customer customer);
}
