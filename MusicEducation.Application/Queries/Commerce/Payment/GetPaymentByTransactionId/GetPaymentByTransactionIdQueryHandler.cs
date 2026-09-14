using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Payment;

public class GetPaymentByTransactionIdQueryHandler
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByTransactionIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> Handle(GetPaymentByTransactionIdQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.TransactionId))
            throw new ArgumentException("شناسه تراکنش الزامی است",
                nameof(query.TransactionId));

        var payment = await _paymentRepository.GetByTransactionIdAsync(query.TransactionId.Trim());

        if (payment is null)
            throw new KeyNotFoundException("پرداخت مربوط به این تراکنش پیدا نشد");

        return payment.ToDto();
    }
}