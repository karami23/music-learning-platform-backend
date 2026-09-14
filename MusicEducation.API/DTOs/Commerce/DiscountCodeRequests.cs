using MusicEducation.Domain.Enums;

namespace MusicEducation.API.DTOs.Commerce.DiscountCode;

public sealed record CreateDiscountCodeRequest(
    string Code,
    DiscountType Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    decimal? MinimumOrderAmount,
    string? MinimumOrderAmountCurrency,
    int? UsageLimit);

public sealed record UpdateDiscountCodeRequest(
    string Code,
    DiscountType Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    decimal? MinimumOrderAmount,
    string? MinimumOrderAmountCurrency,
    int? UsageLimit);