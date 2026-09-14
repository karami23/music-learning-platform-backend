namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed record AddItemToOrderCommand(
    int UserId,
    int OrderId,
    int CourseId);