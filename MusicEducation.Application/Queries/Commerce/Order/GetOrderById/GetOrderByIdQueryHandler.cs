using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Order;

public class GetOrderByIdQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery query)
    {
        if (query.OrderId <= 0)
            throw new ArgumentException("شناسه سفارش معتبر نیست",
                nameof(query.OrderId));

        var order = await _orderRepository.GetByIdAsync(query.OrderId);

        if (order is null)
            throw new KeyNotFoundException("سفارش موردنظر پیدا نشد");

        var items = await _orderItemRepository.GetByOrderIdAsync(order.Id);

        var itemDtos = items
            .Select(x => x.ToDto())
            .ToList();

        return order.ToDto(itemDtos);
    }
}