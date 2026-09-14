using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Payment;

public class GetPaymentByIdQueryHandler
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> Handle(GetPaymentByIdQuery query)
    {
        if (query.PaymentId <= 0)
            throw new ArgumentException("شناسه پرداخت معتبر نیست",
                nameof(query.PaymentId));

        var payment = await _paymentRepository.GetByIdAsync(query.PaymentId);

        if (payment is null)
            throw new KeyNotFoundException("پرداخت موردنظر پیدا نشد");

        return payment.ToDto();
    }
}