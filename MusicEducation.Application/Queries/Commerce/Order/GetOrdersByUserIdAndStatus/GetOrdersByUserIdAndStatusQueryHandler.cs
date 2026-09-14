using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Order;

public class GetOrdersByUserIdAndStatusQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public GetOrdersByUserIdAndStatusQueryHandler(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdAndStatusQuery query)
    {
        if (query.UserId <= 0)
            throw new ArgumentException("شناسه کاربر معتبر نیست",
                nameof(query.UserId));

        if (!Enum.IsDefined(query.Status))
            throw new ArgumentException("وضعیت سفارش معتبر نیست",
                nameof(query.Status));

        var orders = await _orderRepository.GetByUserIdAndStatusAsync(
                query.UserId,
                query.Status);

        var result = new List<OrderDto>();

        foreach (var order in orders)
        {
            var items = await _orderItemRepository.GetByOrderIdAsync(order.Id);

            var itemDtos = items
                .Select(x => x.ToDto())
                .ToList();

            result.Add(order.ToDto(itemDtos));
        }

        return result;
    }
}