using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;
using OrderEntity = MusicEducation.Domain.Entities.Commerce.Order;

namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.OrderNumber))
            throw new ValidationException(
                "شماره سفارش الزامی است");

        var orderNumber = command.OrderNumber.Trim();

        if (await _orderRepository
            .ExistsByOrderNumberAsync(orderNumber))
        {
            throw new ConflictException(
                "این شماره سفارش قبلاً استفاده شده است");
        }

        var order = OrderEntity.Create(
            command.UserId,
            orderNumber);

        await _orderRepository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}