using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed class MarkPaymentAsPaidCommandHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkPaymentAsPaidCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MarkPaymentAsPaidCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.PaymentId <= 0)
            throw new ValidationException(
                "شناسه پرداخت معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.TransactionId))
            throw new ValidationException(
                "شناسه تراکنش الزامی است");

        var payment = await _paymentRepository
            .GetByIdAsync(command.PaymentId);

        if (payment is null)
            throw new NotFoundException(
                "پرداخت موردنظر پیدا نشد");

        var transactionId = command.TransactionId.Trim();

        var existingTransaction =
            await _paymentRepository
                .GetByTransactionIdAsync(transactionId);

        if (existingTransaction is not null &&
            existingTransaction.Id != payment.Id)
        {
            throw new ConflictException(
                "این شناسه تراکنش قبلاً ثبت شده است");
        }

        var order = await _orderRepository
            .GetByIdAsync(payment.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش مربوط به پرداخت پیدا نشد");

        payment.MarkAsPaid(
            transactionId,
            command.ReferenceNumber);

        order.MarkAsPaid();

        await _paymentRepository.UpdateAsync(payment);
        await _orderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}