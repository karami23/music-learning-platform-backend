using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Cart;

public sealed class GetCartByUserIdQueryHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public GetCartByUserIdQueryHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartDto> HandleAsync(GetCartByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException("شناسه کاربر معتبر نیست");

        var cart = await _cartRepository.GetByUserIdAsync(query.UserId);

        if (cart is null)
            throw new NotFoundException("سبد خریدی برای این کاربر پیدا نشد");

        var items = await _cartItemRepository.GetByCartIdAsync(cart.Id);

        return cart.ToDto(items.Select(x => x.ToDto()));
    }
}