namespace estore.api.Abstractions.Mappers;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Responses;

public interface ICustomerMapper :
    IMapper<Customer, CustomerResponse>
{ }
