using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class CartMapping
{
    public static CartDto ToDto(
        this Cart cart,
        IEnumerable<CartItemDto> items)
    {
        return new CartDto(
            cart.Id,
            cart.UserId,
            items.ToList().AsReadOnly()
        );
    }
}