using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Commerce;

public record DiscountCodeDto(
    int Id,
    string Code,
    DiscountType Type,
    decimal Value,
    decimal? MinimumOrderAmount,
    string? MinimumOrderAmountCurrency,
    int? UsageLimit,
    int UsedCount,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
);