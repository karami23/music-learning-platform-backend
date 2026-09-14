using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Carts.Cart.AddItemToCart;

public sealed class AddItemToCartCommandHandler
{
    private readonly ICartRepository _cartRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(
        AddItemToCartCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var cart = await _cartRepository
            .GetByUserIdAsync(command.UserId);

        if (cart is null)
            throw new NotFoundException(
                "سبد خریدی برای این کاربر پیدا نشد");

        if (cart is null)
            throw new NotFoundException(
                "سبد خرید موردنظر پیدا نشد");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException(
                "دوره موردنظر پیدا نشد");

        var cartItem = CartItem.Create(
            cart.Id,
            course.Id);

        cart.AddItem(cartItem);

        await _cartRepository.UpdateAsync(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cartItem.Id;
    }
}