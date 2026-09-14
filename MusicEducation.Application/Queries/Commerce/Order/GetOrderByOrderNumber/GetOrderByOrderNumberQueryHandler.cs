using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Order;

public class GetOrderByOrderNumberQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public GetOrderByOrderNumberQueryHandler(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<OrderDto> Handle(GetOrderByOrderNumberQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.OrderNumber))
            throw new ArgumentException("شماره سفارش الزامی است",
                nameof(query.OrderNumber));

        var order = await _orderRepository.GetByOrderNumberAsync(query.OrderNumber.Trim());

        if (order is null)
            throw new KeyNotFoundException("سفارش موردنظر پیدا نشد");

        var items = await _orderItemRepository.GetByOrderIdAsync(order.Id);

        var itemDtos = items
            .Select(x => x.ToDto())
            .ToList();

        return order.ToDto(itemDtos);
    }
}