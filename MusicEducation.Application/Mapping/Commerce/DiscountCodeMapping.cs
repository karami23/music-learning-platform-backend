using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Application.Mappings.Commerce;

public static class DiscountCodeMapping
{
    public static DiscountCodeDto ToDto(this DiscountCode discountCode)
    {
        return new DiscountCodeDto(
            discountCode.Id,
            discountCode.Code,
            discountCode.Type,
            discountCode.Value,
            discountCode.MinimumOrderAmount?.Amount,
            discountCode.MinimumOrderAmount?.Currency,
            discountCode.UsageLimit,
            discountCode.UsedCount,
            discountCode.ValidityPeriod.StartDate,
            discountCode.ValidityPeriod.EndDate,
            discountCode.IsActive
        );
    }
}