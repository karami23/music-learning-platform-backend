using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Payment;

public class GetPaymentByOrderIdQueryHandler
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByOrderIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> Handle(GetPaymentByOrderIdQuery query)
    {
        if (query.OrderId <= 0)
            throw new ArgumentException("شناسه سفارش معتبر نیست",
                nameof(query.OrderId));

        var payment = await _paymentRepository.GetByOrderIdAsync(query.OrderId);

        if (payment is null)
            throw new KeyNotFoundException("پرداخت مربوط به سفارش پیدا نشد");

        return payment.ToDto();
    }
}