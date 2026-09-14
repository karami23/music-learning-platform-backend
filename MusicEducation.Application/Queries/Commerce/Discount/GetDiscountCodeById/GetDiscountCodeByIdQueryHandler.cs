using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.DiscountCode;

public class GetDiscountCodeByIdQueryHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;

    public GetDiscountCodeByIdQueryHandler(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }

    public async Task<DiscountCodeDto> Handle(GetDiscountCodeByIdQuery query)
    {
        if (query.DiscountCodeId <= 0)
            throw new ArgumentException("شناسه کد تخفیف معتبر نیست",
                nameof(query.DiscountCodeId));

        var discountCode = await _discountCodeRepository.GetByIdAsync(query.DiscountCodeId);

        if (discountCode is null)
            throw new KeyNotFoundException("کد تخفیف موردنظر پیدا نشد");

        return discountCode.ToDto();
    }
}