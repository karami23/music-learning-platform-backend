using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class CartItemMapping
{
    public static CartItemDto ToDto(this CartItem cartItem)
    {
        return new CartItemDto(
            cartItem.Id,
            cartItem.CartId,
            cartItem.CourseId
        );
    }
}