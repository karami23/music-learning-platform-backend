using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Carts.Cart.ClearCart;

public sealed class ClearCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        ClearCartCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        var cart = await _cartRepository
            .GetByUserIdAsync(command.UserId);

        if (cart is null)
            throw new NotFoundException(
                "سبد خرید موردنظر پیدا نشد");

        cart.Clear();

        await _cartRepository.UpdateAsync(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}