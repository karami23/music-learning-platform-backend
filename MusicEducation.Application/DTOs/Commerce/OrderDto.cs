using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Commerce;

public record OrderDto(
    int Id,
    int UserId,
    string OrderNumber,
    OrderStatus Status,
    decimal Subtotal,
    string SubtotalCurrency,
    decimal DiscountAmount,
    string DiscountCurrency,
    decimal TotalAmount,
    string TotalCurrency,
    int? DiscountCodeId,
    IReadOnlyCollection<OrderItemDto> Items
);