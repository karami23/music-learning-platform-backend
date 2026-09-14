namespace MusicEducation.API.DTOs.Commerce.Order;

public sealed record CreateOrderRequest(
    string OrderNumber);

public sealed record AddItemToOrderRequest(
    int CourseId);

public sealed record ApplyDiscountToOrderRequest(
    string DiscountCode);

public sealed record RemoveItemFromOrderRequest(
    int CourseId);