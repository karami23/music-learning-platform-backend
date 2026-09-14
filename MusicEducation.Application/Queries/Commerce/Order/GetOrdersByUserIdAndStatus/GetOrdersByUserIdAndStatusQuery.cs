using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Queries.Commerce.Order;

public record GetOrdersByUserIdAndStatusQuery(
    int UserId,
    OrderStatus Status
);