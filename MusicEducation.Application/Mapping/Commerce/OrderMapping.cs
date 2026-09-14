using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order order, IEnumerable<OrderItemDto> items)
    {
        return new OrderDto(
            order.Id,
            order.UserId,
            order.OrderNumber,
            order.Status,
            order.Subtotal.Amount,
            order.Subtotal.Currency,
            order.DiscountAmount.Amount,
            order.DiscountAmount.Currency,
            order.TotalAmount.Amount,
            order.TotalAmount.Currency,
            order.DiscountCodeId,
            items.ToList().AsReadOnly()
        );
    }
}