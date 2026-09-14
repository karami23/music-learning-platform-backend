using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed class MarkOrderAsPaidCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsPaidCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MarkOrderAsPaidCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.OrderId <= 0)
            throw new ValidationException(
                "شناسه سفارش معتبر نیست");

        var order = await _orderRepository
            .GetByIdAsync(command.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش موردنظر پیدا نشد");

        order.MarkAsPaid();

        await _orderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}