namespace estore.api.Abstractions.Mappers;

using estore.api.Common;
using estore.api.Models.Aggregates.Orders;
using estore.common.Models.Responses;

public interface IOrderMapper :
    IMapper<Order, OrderResponse>
{ }
