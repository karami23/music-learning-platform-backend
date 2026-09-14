namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed record RemoveItemFromOrderCommand(
    int UserId,
    int OrderId,
    int CourseId);