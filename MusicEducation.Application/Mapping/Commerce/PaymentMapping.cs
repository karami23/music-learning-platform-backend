using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class PaymentMapping
{
    public static PaymentDto ToDto(this Payment payment)
    {
        return new PaymentDto(
            payment.Id,
            payment.OrderId,
            payment.Amount.Amount,
            payment.Amount.Currency,
            payment.Status,
            payment.TransactionId,
            payment.ReferenceNumber,
            payment.PaidAt
        );
    }
}