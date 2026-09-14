using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed class RemoveItemFromOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemFromOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RemoveItemFromOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.OrderId <= 0)
            throw new ValidationException(
                "شناسه سفارش معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order is null)
            throw new NotFoundException("سفارش موردنظر پیدا نشد");

        if (order.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این سفارش را ندارید");

        order.RemoveItem(command.CourseId);

        await _orderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}