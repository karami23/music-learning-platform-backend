using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.Payment;

public class GetPaymentsByUserIdQueryHandler
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentsByUserIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<PaymentDto>> Handle(GetPaymentsByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ArgumentException("شناسه کاربر معتبر نیست",
                nameof(query.UserId));

        var payments = await _paymentRepository.GetByUserIdAsync(query.UserId);

        return payments
            .Select(x => x.ToDto())
            .ToList();
    }
}