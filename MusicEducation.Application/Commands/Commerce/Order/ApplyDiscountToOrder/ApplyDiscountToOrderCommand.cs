namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed record ApplyDiscountToOrderCommand(
    int UserId,
    int OrderId,
    string DiscountCode);