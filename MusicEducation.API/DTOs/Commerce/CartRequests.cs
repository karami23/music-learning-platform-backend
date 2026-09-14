namespace MusicEducation.API.DTOs.Commerce.Cart;

public sealed record AddItemToCartRequest(
    int CourseId);

public sealed record RemoveItemFromCartRequest(
    int CourseId);