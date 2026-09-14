using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.DiscountCode;

public class GetDiscountCodeByCodeQueryHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;

    public GetDiscountCodeByCodeQueryHandler(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }

    public async Task<DiscountCodeDto> Handle(GetDiscountCodeByCodeQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Code))
            throw new ArgumentException("کد تخفیف الزامی است",
                nameof(query.Code));

        var discountCode = await _discountCodeRepository.GetByCodeAsync(query.Code.Trim());

        if (discountCode is null)
            throw new KeyNotFoundException("کد تخفیف موردنظر پیدا نشد");

        return discountCode.ToDto();
    }
}