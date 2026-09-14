using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed class CancelPaymentCommandHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CancelPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

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

        if (order.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این پرداخت را ندارید");

        payment.Cancel();

        await _paymentRepository.UpdateAsync(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}