using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;
using PaymentEntity =
    MusicEducation.Domain.Entities.Commerce.Payment;

namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed class CreatePaymentCommandHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreatePaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");
        if (command.OrderId <= 0)
            throw new ValidationException(
                "شناسه سفارش معتبر نیست");

        var order = await _orderRepository
            .GetByIdAsync(command.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش موردنظر پیدا نشد");

        if (order.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این سفارش را ندارید");

        if (order.Status != Domain.Enums.OrderStatus.Pending)
            throw new ConflictException(
                "فقط برای سفارش در انتظار پرداخت می‌توان پرداخت ایجاد کرد");

        var existingPayment = await _paymentRepository
            .GetByOrderIdAsync(command.OrderId);

        if (existingPayment is not null)
            throw new ConflictException(
                "برای این سفارش قبلاً پرداخت ایجاد شده است");

        var payment = PaymentEntity.Create(
            order.Id,
            order.TotalAmount);

        await _paymentRepository.AddAsync(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}