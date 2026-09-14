using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Cart;

public sealed class GetCartByIdQueryHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public GetCartByIdQueryHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartDto> HandleAsync(GetCartByIdQuery query)
    {
        if (query.CartId <= 0)
            throw new ValidationException("شناسه سبد خرید معتبر نیست");

        var cart = await _cartRepository.GetByIdAsync(query.CartId);

        if (cart is null)
            throw new NotFoundException("سبد خرید موردنظر پیدا نشد");

        var items = await _cartItemRepository.GetByCartIdAsync(cart.Id);

        return cart.ToDto(items.Select(x => x.ToDto()));
    }
}