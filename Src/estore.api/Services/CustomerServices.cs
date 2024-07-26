namespace estore.api.Services;

using System.Net;
using System.Threading.Tasks;
using estore.api.Abstractions.Mappers;
using estore.api.Abstractions.Services;
using estore.api.Common;
using estore.api.Models.Aggregates.Customer;
using estore.common.Common.Pagination;
using estore.common.Common.Results;
using estore.common.Models.Requests;
using estore.common.Models.Responses;
using FluentValidation;
using Models.Aggregates.Customer.ValueObjects;

public class CustomerServices(
    ICustomerRepository customerRepository,
    ICustomerMapper customerMapper,
    IPagedList<CustomerResponse> paged,
    IUnitOfWork unitOfWork,
    IValidator<CreateCustomerRequest> validator) : ICustomerServices
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerMapper _customerMapper = customerMapper;
    private readonly IPagedList<CustomerResponse> _paged = paged;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<CreateCustomerRequest> _validator = validator;

    public async Task<Result<CreateCustomerResponse>> CreateCustomer(CreateCustomerRequest customerRequest)
    {
        var result = await _validator.ValidateAsync(customerRequest);

        if (result.IsValid)
        {
            var customer = _customerMapper.Map(customerRequest);

            _customerRepository.Add(customer);

            await _unitOfWork.CompleteAsync();

            return Result<CreateCustomerResponse>.SuccessResult(_customerMapper.Mapper(customer));
        }
        return Result<CreateCustomerResponse>.FailedResult(result.Errors.Select(x => x.ErrorMessage).First(), HttpStatusCode.BadRequest);
    }

    public async Task<Result<CustomerResponse>> GetCustomerByCustomerId(string customerId)
    {
        var customer = await _customerRepository
            .FindByConditionAsync(x => x.Id == new CustomerId(customerId));

        return customer.Any() ?
           Result<CustomerResponse>
            .SuccessResult(_customerMapper.Map(customer.SingleOrDefault()!)) :
           Result<CustomerResponse>
            .FailedResult("Customer not found with this Id", HttpStatusCode.NotFound);
    }

    public PagedList<CustomerResponse> GetCustomers(SearchCustomerRequest search)
    {
        var customers = _customerRepository.GetAll();

        if (!string.IsNullOrEmpty(search.CustomerId))
        {
            customers = customers.Where(x => x.Id == new CustomerId(search.CustomerId));
        }

        if (!string.IsNullOrEmpty(search.ContactName))
        {
            customers = customers.Where(x => x.ContactName == search.ContactName);
        }

        if (!string.IsNullOrEmpty(search.ContactTitle))
        {
            customers = customers.Where(x => x.ContactTitle == search.ContactTitle);
        }

        if (!string.IsNullOrEmpty(search.CompanyName))
        {
            customers = customers.Where(x => x.CompanyName == search.CompanyName);
        }

        return _paged.ToPagedList(customers.Distinct().Select(_customerMapper.Map).AsQueryable(), search.PageNumber, search.PageSize);
    }

    public async Task<Result> DeleteCustomer(string customerId)
    {
        var customer = await _customerRepository.FindByConditionAsync(x => x.Id == new CustomerId(customerId.Trim()));

        if (customer.Any())
        {
            await _customerRepository.DeleteCustomer(customerId.Trim());

            return Result.SuccessResult();
        }

        return Result.FailedResult(["Customer not found with this Id"], HttpStatusCode.NotFound);
    }
}
