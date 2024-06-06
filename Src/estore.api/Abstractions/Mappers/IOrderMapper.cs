namespace estore.api.Abstractions.Mappers;

using estore.api.Common.Models;
using estore.api.Models.Aggregates.Orders;
using estore.api.Models.Responses;

public interface IOrderMapper :
    IMapper<Order, CustomerOrderResponse>
{ }
