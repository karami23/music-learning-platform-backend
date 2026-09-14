using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class OrderItemMapping
{
    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            item.Id,
            item.OrderId,
            item.CourseId,
            item.CourseTitle,
            item.UnitPrice.Amount,
            item.UnitPrice.Currency
        );
    }
}