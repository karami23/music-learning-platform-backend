using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Order;

public class GetOrdersByUserIdQueryHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public GetOrdersByUserIdQueryHandler(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ArgumentException("شناسه کاربر معتبر نیست",
                nameof(query.UserId));

        var orders = await _orderRepository.GetByUserIdAsync(query.UserId);

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