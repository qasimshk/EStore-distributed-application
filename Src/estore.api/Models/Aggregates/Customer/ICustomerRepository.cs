namespace estore.api.Models.Aggregates.Customer;

using estore.api.Common;

public interface ICustomerRepository : IRepository<Customer>
{
    Task DeleteCustomer(string customerId);
}
