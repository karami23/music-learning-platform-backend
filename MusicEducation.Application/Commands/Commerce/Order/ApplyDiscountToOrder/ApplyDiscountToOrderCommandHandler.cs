using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed class ApplyDiscountToOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApplyDiscountToOrderCommandHandler(
        IOrderRepository orderRepository,
        IDiscountCodeRepository discountCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _discountCodeRepository = discountCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ApplyDiscountToOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.OrderId <= 0)
            throw new ValidationException(
                "شناسه سفارش معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.DiscountCode))
            throw new ValidationException(
                "کد تخفیف الزامی است");

        var order = await _orderRepository
            .GetByIdAsync(command.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش موردنظر پیدا نشد");

        var discountCode = await _discountCodeRepository
            .GetByCodeAsync(command.DiscountCode.Trim());

        if (discountCode is null)
            throw new NotFoundException(
                "کد تخفیف موردنظر پیدا نشد");

        if (!discountCode.IsValid(DateTime.UtcNow))
            throw new ValidationException(
                "کد تخفیف معتبر یا فعال نیست");

        var discountAmount =
            discountCode.CalculateDiscount(order.Subtotal);

        order.ApplyDiscount(
            discountCode.Id,
            discountAmount);

        discountCode.RegisterUsage();

        await _orderRepository.UpdateAsync(order);
        await _discountCodeRepository.UpdateAsync(discountCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}