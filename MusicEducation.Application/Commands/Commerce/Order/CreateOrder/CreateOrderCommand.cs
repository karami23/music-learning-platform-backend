namespace MusicEducation.Application.Commands.Commerce.Order;

public record CreateOrderCommand(
    int UserId,
    string OrderNumber
);