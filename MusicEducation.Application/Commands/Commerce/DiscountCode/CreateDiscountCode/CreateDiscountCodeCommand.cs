using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Commerce.DiscountCode;

public record CreateDiscountCodeCommand(
    string Code,
    DiscountType Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    decimal? MinimumOrderAmount = null,
    string? MinimumOrderAmountCurrency = null,
    int? UsageLimit = null
);