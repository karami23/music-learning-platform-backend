namespace MusicEducation.Application.Commands.Carts.Cart.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand(
    int UserId,
    int CourseId);