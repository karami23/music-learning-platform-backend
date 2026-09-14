using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed class MarkPaymentAsFailedCommandHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkPaymentAsFailedCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MarkPaymentAsFailedCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.PaymentId <= 0)
            throw new ValidationException(
                "شناسه پرداخت معتبر نیست");

        var payment = await _paymentRepository
            .GetByIdAsync(command.PaymentId);

        if (payment is null)
            throw new NotFoundException(
                "پرداخت موردنظر پیدا نشد");

        var order = await _orderRepository
            .GetByIdAsync(payment.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش مربوط به پرداخت پیدا نشد");

        payment.MarkAsFailed();
        order.MarkAsFailed();

        await _paymentRepository.UpdateAsync(payment);
        await _orderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}