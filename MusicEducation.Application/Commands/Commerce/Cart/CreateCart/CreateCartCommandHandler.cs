using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;
using CartEntity = MusicEducation.Domain.Entities.Commerce.Cart;

namespace MusicEducation.Application.Commands.Carts.Cart.CreateCart;

public sealed class CreateCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(
        CreateCartCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        var existingCart = await _cartRepository
            .GetByUserIdAsync(command.UserId);

        if (existingCart is not null)
            throw new ConflictException(
                "این کاربر قبلاً سبد خرید دارد");

        var cart = CartEntity.Create(command.UserId);

        await _cartRepository.AddAsync(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cart.Id;
    }
}