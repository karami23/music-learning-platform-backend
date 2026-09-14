namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed record CancelOrderCommand(
    int UserId,
    int OrderId);